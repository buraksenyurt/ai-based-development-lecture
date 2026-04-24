# Weather MCP Server

A Python [FastMCP](https://gofastmcp.com) server that exposes the Weather Statistic API as MCP tools over **Streamable HTTP** transport.

## Overview

```
MCP Client (Claude, etc.)
        │  HTTP  POST /mcp
        ▼
Weather MCP Server  (this project, port 8010)
        │  HTTP  GET /weather/…
        ▼
Weather Statistic API  (lesson10/WeatherApi, port 7010)
```

## Prerequisites

- Python 3.10 or newer
- The [Weather Statistic API](../WeatherApi/WeatherStatisticApiDesign.md) running at `http://localhost:7010`

## Install

```bash
# create and activate a virtual environment (recommended)
python -m venv .venv
.venv\Scripts\activate      # Windows
# source .venv/bin/activate   # macOS / Linux

pip install -r requirements.txt
```

## Configure

```bash
copy .env.example .env
```

Edit `.env` if necessary:

| Variable               | Default               | Description                            |
|------------------------|-----------------------|----------------------------------------|
| `WEATHER_API_BASE_URL` | `http://localhost:7010` | Base URL of the Weather Statistic API |
| `MCP_HOST`             | `127.0.0.1`           | Host for the MCP server                |
| `MCP_PORT`             | `8010`                | Port for the MCP server                |

## Run

```bash
python main.py
```

The server starts and prints:

```
[MCP SERVER INFO] Weather MCP Server starting ...
[MCP SERVER INFO] Backend API : http://localhost:7010
[MCP SERVER INFO] MCP endpoint: http://127.0.0.1:8010/mcp
```

Connect any MCP client to the **Streamable HTTP** endpoint:

```
http://127.0.0.1:8010/mcp
```

## Available Tools

### `discover_weather_api_tools()`
Fetches `/openapi.json` from the backend and returns all discovered GET endpoints with their query parameters. Use this first to inspect what data can be queried.

---

### `get_weather_statistics(location, start_date?, end_date?, weather_condition?)`
Aggregated weather statistics for a location.

| Parameter | Required | Description |
|---|---|---|
| `location` | yes | City name, postal code, or `lat,lon` |
| `start_date` | no | `YYYY-MM-DD` — omit both dates to use full dataset range |
| `end_date` | no | `YYYY-MM-DD` — must be paired with `start_date` |
| `weather_condition` | no | `rain` \| `snow` \| `sunny` \| `cloudy` |

---

### `get_average_temperature(location, start_date?, end_date?)`
Average temperature in Celsius for a location.

| Parameter | Required | Description |
|---|---|---|
| `location` | yes | City name, postal code, or `lat,lon` |
| `start_date` | no | `YYYY-MM-DD` |
| `end_date` | no | `YYYY-MM-DD` |

---

### `get_precipitation(location, start_date?, end_date?)`
Total precipitation in millimetres for a location.

| Parameter | Required | Description |
|---|---|---|
| `location` | yes | City name, postal code, or `lat,lon` |
| `start_date` | no | `YYYY-MM-DD` |
| `end_date` | no | `YYYY-MM-DD` |

---

### `get_continent_statistics(continent, start_date?, end_date?)`
Weather statistics for all capital cities in a continent, sorted alphabetically by city name.

| Parameter | Required | Description |
|---|---|---|
| `continent` | yes | e.g. `Europe`, `Asia`, `North America` |
| `start_date` | no | `YYYY-MM-DD` |
| `end_date` | no | `YYYY-MM-DD` |

---

### `weather_api_health_check()`
Calls `GET /health` on the backend. Returns `{"status": "ok"}` when healthy.

## Error Handling

Every tool returns a structured dict. On backend errors the shape is:

```json
{
  "status_code": 404,
  "error": {
    "type": "BACKEND_ERROR",
    "message": "Weather API returned a non-success status code.",
    "endpoint": "/weather/statistics",
    "payload": { ... }
  }
}
```

On network-level failures `status_code` is `null` and `error.type` is `REQUEST_ERROR`.

## Sample Prompts

```
What are the weather statistics for London between 2026-01-10 and 2026-01-11?
```

```
Which European capitals had the most precipitation in January 2026?
```

```
Is the weather backend healthy?
```
