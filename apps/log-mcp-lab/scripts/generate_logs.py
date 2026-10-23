"""Generate deterministic OpenTelemetry-aligned JSONL logs for the lab."""

import argparse
import json
import random
from datetime import datetime, timedelta, timezone
from pathlib import Path


SERVICES = ("gateway-api", "orders-api", "inventory-api")
SEVERITY_NUMBERS = {"INFO": 9, "WARN": 13, "ERROR": 17}


def create_event(index, timestamp, service, severity, body, event_name, scenario, **attributes):
    trace_id = f"{index + 1:032x}"
    span_id = f"{index + 1:016x}"
    event_attributes = {
        "event.domain": event_name.rsplit(".", 1)[0],
        "event.outcome": "failure" if severity == "ERROR" else "success",
        "host.name": f"lab-node-{(index % 3) + 1:02d}",
        "scenario": scenario,
        **attributes,
    }
    return {
        "timestamp": timestamp.isoformat(timespec="milliseconds").replace("+00:00", "Z"),
        "observed_timestamp": timestamp.isoformat(timespec="milliseconds").replace("+00:00", "Z"),
        "severity_number": SEVERITY_NUMBERS[severity],
        "severity_text": severity,
        "body": body,
        "event_name": event_name,
        "service_name": service,
        "service_version": "1.0.0",
        "deployment_environment": "lab",
        "trace_id": trace_id,
        "span_id": span_id,
        "resource": {
            "service.name": service,
            "service.version": "1.0.0",
            "deployment.environment": "lab",
        },
        "attributes": event_attributes,
    }


def generate_events(seed, baseline_events):
    randomizer = random.Random(seed)
    start = datetime(2026, 10, 23, 3, 0, tzinfo=timezone.utc)
    events = []

    for index in range(baseline_events):
        service = SERVICES[index % len(SERVICES)]
        timestamp = start + timedelta(seconds=index)
        duration = randomizer.randint(40, 180)
        events.append(
            create_event(
                index,
                timestamp,
                service,
                "INFO",
                "Request completed",
                "http.server.request",
                "baseline",
                **{
                    "http.request.method": "GET",
                    "http.response.status_code": 200,
                    "duration_ms": duration,
                },
            )
        )

    index = len(events)
    for offset in range(12):
        timestamp = start + timedelta(seconds=index + offset)
        events.append(
            create_event(
                index + offset,
                timestamp,
                "orders-api",
                "WARN",
                "Request latency exceeded expected range",
                "http.server.request",
                "latency_degradation",
                **{
                    "http.request.method": "POST",
                    "http.response.status_code": 200,
                    "duration_ms": 750 + (offset * 50),
                },
            )
        )

    index = len(events)
    for offset in range(8):
        timestamp = start + timedelta(seconds=index + offset)
        events.append(
            create_event(
                index + offset,
                timestamp,
                "inventory-api",
                "ERROR",
                "Database connection timed out",
                "database.query",
                "error_burst",
                **{"error.type": "TimeoutError", "duration_ms": 2_000},
            )
        )

    index = len(events)
    for offset in range(6):
        timestamp = start + timedelta(seconds=index + offset)
        events.append(
            create_event(
                index + offset,
                timestamp,
                "orders-api",
                "ERROR",
                "Inventory dependency returned an error",
                "dependency.call",
                "dependency_failure",
                **{
                    "error.type": "DependencyError",
                    "dependency.name": "inventory-api",
                    "http.response.status_code": 503,
                },
            )
        )

    index = len(events)
    for offset in range(6):
        timestamp = start + timedelta(seconds=index + offset)
        events.append(
            create_event(
                index + offset,
                timestamp,
                SERVICES[offset % len(SERVICES)],
                "INFO",
                "Service recovered and request completed",
                "service.recovered",
                "recovery",
                **{"duration_ms": randomizer.randint(50, 160)},
            )
        )
    return events


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--output", type=Path, default=Path("data/generated/lab-logs.jsonl"))
    parser.add_argument("--seed", type=int, default=42)
    parser.add_argument("--baseline-events", type=int, default=120)
    args = parser.parse_args()

    events = generate_events(args.seed, args.baseline_events)
    args.output.parent.mkdir(parents=True, exist_ok=True)
    with args.output.open("w", encoding="utf-8", newline="\n") as output:
        for event in events:
            output.write(json.dumps(event, separators=(",", ":")) + "\n")
    print(f"Wrote {len(events)} events to {args.output}")


if __name__ == "__main__":
    main()
