from __future__ import annotations

import os
from typing import Any

import httpx
from fastmcp import FastMCP

# ---------------------------------------------------------------------------
# Configuration
# ---------------------------------------------------------------------------

WEATHER_API_BASE_URL: str = os.getenv("WEATHER_API_BASE_URL", "http://localhost:7010").rstrip("/")
MCP_HOST: str = os.getenv("MCP_HOST", "127.0.0.1")
MCP_PORT: int = int(os.getenv("MCP_PORT", "8010"))

# ---------------------------------------------------------------------------
# FastMCP server
# ---------------------------------------------------------------------------

mcp = FastMCP("Weather MCP Server")

# ---------------------------------------------------------------------------
# Internal helpers
# ---------------------------------------------------------------------------


def _build_url(path: str) -> str:
    normalized = path if path.startswith("/") else f"/{path}"
    return f"{WEATHER_API_BASE_URL}{normalized}"


def _clean_params(params: dict[str, Any]) -> dict[str, Any]:
    """Remove None values so they are not sent as query string literals."""
    return {k: v for k, v in params.items() if v is not None}


def _is_success(status_code: int | None) -> bool:
    return status_code is not None and 200 <= status_code < 300


def _error_response(endpoint: str, response: dict[str, Any]) -> dict[str, Any]:
    return {
        "status_code": response.get("status_code"),
        "error": {
            "type": "BACKEND_ERROR",
            "message": "Weather API returned a non-success status code.",
            "endpoint": endpoint,
            "payload": response.get("payload"),
        },
    }


async def _backend_get(
    path: str,
    params: dict[str, Any] | None = None,
) -> dict[str, Any]:
    """
    Issue an async GET request against the backend Weather API.

    Returns a normalized dict::

        {
          "status_code": int | None,
          "payload": dict | str
        }

    Payload is parsed JSON when possible, otherwise raw text.
    On a network-level error status_code is None.
    """
    url = _build_url(path)
    query = _clean_params(params or {})

    try:
        async with httpx.AsyncClient(timeout=20.0) as client:
            res = await client.get(url, params=query)
    except httpx.RequestError as exc:
        return {
            "status_code": None,
            "payload": {
                "type": "REQUEST_ERROR",
                "message": str(exc),
                "url": url,
            },
        }

    try:
        payload: Any = res.json()
    except ValueError:
        payload = res.text

    return {"status_code": res.status_code, "payload": payload}


async def _fetch_openapi() -> dict[str, Any]:
    """
    Fetch /openapi.json from the backend and derive available GET operations.

    Returns a dict with keys:
        - ``status_code``
        - ``payload.base_url``
        - ``payload.get_endpoints``  (list of endpoint descriptors)
    """
    raw = await _backend_get("/openapi.json")
    if not _is_success(raw.get("status_code")):
        return _error_response("/openapi.json", raw)

    spec = raw.get("payload")
    if not isinstance(spec, dict):
        return {
            "status_code": raw.get("status_code"),
            "error": {
                "type": "OPENAPI_PARSE_ERROR",
                "message": "OpenAPI payload is not a JSON object.",
                "payload": spec,
            },
        }

    paths: Any = spec.get("paths", {})
    if not isinstance(paths, dict):
        return {
            "status_code": raw.get("status_code"),
            "error": {
                "type": "OPENAPI_PARSE_ERROR",
                "message": "OpenAPI paths field is not a JSON object.",
                "payload": paths,
            },
        }

    discovered: list[dict[str, Any]] = []
    for path, path_item in paths.items():
        if not isinstance(path_item, dict):
            continue
        get_op = path_item.get("get")
        if not isinstance(get_op, dict):
            continue

        raw_params: Any = get_op.get("parameters", [])
        normalized_params: list[dict[str, Any]] = []
        if isinstance(raw_params, list):
            for p in raw_params:
                if not isinstance(p, dict):
                    continue
                schema = p.get("schema") if isinstance(p.get("schema"), dict) else {}
                normalized_params.append(
                    {
                        "name": p.get("name"),
                        "in": p.get("in"),
                        "required": bool(p.get("required", False)),
                        "type": schema.get("type"),
                        "description": p.get("description"),
                    }
                )

        discovered.append(
            {
                "path": path,
                "summary": get_op.get("summary"),
                "parameters": normalized_params,
            }
        )

    return {
        "status_code": raw.get("status_code"),
        "payload": {
            "base_url": WEATHER_API_BASE_URL,
            "get_endpoints": discovered,
        },
    }


# ---------------------------------------------------------------------------
# MCP Tools
# ---------------------------------------------------------------------------


@mcp.tool
async def discover_weather_api_tools() -> dict[str, Any]:
    """
    Discover all available Weather API GET endpoints and their parameters.

    Fetches the backend /openapi.json schema and returns a structured list of
    discovered paths, summaries, and query parameters. Use this tool first to
    understand what weather data can be queried.

    Returns:
        Structured mapping with ``base_url`` and ``get_endpoints`` list,
        or an error dict when the backend is unreachable.
    """
    return await _fetch_openapi()


@mcp.tool
async def get_weather_statistics(
    location: str,
    start_date: str | None = None,
    end_date: str | None = None,
    weather_condition: str | None = None,
) -> dict[str, Any]:
    """
    Retrieve aggregated weather statistics for a location.

    Args:
        location: City name, postal code, or geographic coordinates (lat,lon).
        start_date: Start of the date range in YYYY-MM-DD format. Optional –
            when omitted together with end_date the full dataset range is used.
        end_date: End of the date range in YYYY-MM-DD format. Optional –
            must be provided together with start_date.
        weather_condition: Filter records by condition. Accepted values:
            ``rain``, ``snow``, ``sunny``, ``cloudy``. Optional.

    Returns:
        Dict with ``average_temperature``, ``total_precipitation``,
        ``weather_conditions`` counts, and the applied date range.
    """
    endpoint = "/weather/statistics"
    response = await _backend_get(
        endpoint,
        {
            "location": location,
            "start_date": start_date,
            "end_date": end_date,
            "weather_condition": weather_condition,
        },
    )
    if not _is_success(response.get("status_code")):
        return _error_response(endpoint, response)
    return response


@mcp.tool
async def get_average_temperature(
    location: str,
    start_date: str | None = None,
    end_date: str | None = None,
) -> dict[str, Any]:
    """
    Retrieve the average temperature for a location.

    Args:
        location: City name, postal code, or geographic coordinates (lat,lon).
        start_date: Start of the date range in YYYY-MM-DD format. Optional.
        end_date: End of the date range in YYYY-MM-DD format. Optional.

    Returns:
        Dict with ``location``, ``start_date``, ``end_date``, and
        ``average_temperature`` in Celsius.
    """
    endpoint = "/weather/statistics/average-temperature"
    response = await _backend_get(
        endpoint,
        {
            "location": location,
            "start_date": start_date,
            "end_date": end_date,
        },
    )
    if not _is_success(response.get("status_code")):
        return _error_response(endpoint, response)
    return response


@mcp.tool
async def get_precipitation(
    location: str,
    start_date: str | None = None,
    end_date: str | None = None,
) -> dict[str, Any]:
    """
    Retrieve total precipitation levels for a location.

    Args:
        location: City name, postal code, or geographic coordinates (lat,lon).
        start_date: Start of the date range in YYYY-MM-DD format. Optional.
        end_date: End of the date range in YYYY-MM-DD format. Optional.

    Returns:
        Dict with ``location``, ``start_date``, ``end_date``, and
        ``total_precipitation`` in millimetres.
    """
    endpoint = "/weather/statistics/precipitation"
    response = await _backend_get(
        endpoint,
        {
            "location": location,
            "start_date": start_date,
            "end_date": end_date,
        },
    )
    if not _is_success(response.get("status_code")):
        return _error_response(endpoint, response)
    return response


@mcp.tool
async def get_continent_statistics(
    continent: str,
    start_date: str | None = None,
    end_date: str | None = None,
) -> dict[str, Any]:
    """
    Retrieve weather statistics for all capital cities in a continent,
    sorted alphabetically by city name.

    Args:
        continent: Continent name. Supported values include ``Europe``,
            ``Asia``, ``North America``.
        start_date: Start of the date range in YYYY-MM-DD format. Optional.
        end_date: End of the date range in YYYY-MM-DD format. Optional.

    Returns:
        Dict with ``continent``, applied date range, and
        ``capital_weather_conditions`` keyed by city name.
    """
    endpoint = "/weather/statistics/continent"
    response = await _backend_get(
        endpoint,
        {
            "continent": continent,
            "start_date": start_date,
            "end_date": end_date,
        },
    )
    if not _is_success(response.get("status_code")):
        return _error_response(endpoint, response)
    return response


@mcp.tool
async def weather_api_health_check() -> dict[str, Any]:
    """
    Check the health and availability of the backend Weather API service.

    Calls GET /health on the configured backend base URL.

    Returns:
        Dict with ``status_code`` and the backend ``payload`` (normally
        ``{"status": "ok"}``).  Returns an error dict when the backend is
        unreachable.
    """
    endpoint = "/health"
    response = await _backend_get(endpoint)
    if not _is_success(response.get("status_code")):
        return _error_response(endpoint, response)
    return response


# ---------------------------------------------------------------------------
# Entry-point
# ---------------------------------------------------------------------------

if __name__ == "__main__":
    print(f"[MCP SERVER INFO] Weather MCP Server starting ...")
    print(f"[MCP SERVER INFO] Backend API : {WEATHER_API_BASE_URL}")
    print(f"[MCP SERVER INFO] MCP endpoint: http://{MCP_HOST}:{MCP_PORT}/mcp")
    mcp.run(transport="http", host=MCP_HOST, port=MCP_PORT)
