# Weather Statistic API - Lesson10

This project is a Node.js + TypeScript REST API implementation of the design documented in [WeatherStatisticApiDesign.md](./WeatherStatisticApiDesign.md).

## Tech Stack

- Node.js
- Express
- TypeScript
- Node test runner + Supertest

## Prerequisites

- Node.js 18+
- npm

## Setup

```bash
npm install
```

Optional environment configuration:

```bash
copy .env.example .env
```

## Run

Development mode:

```bash
npm run dev
```

Build and start:

```bash
npm run build
npm start
```

Default URL: `http://localhost:7010`

## API Endpoints

- `GET /weather/statistics`
- `GET /weather/statistics/average-temperature`
- `GET /weather/statistics/precipitation`
- `GET /weather/statistics/continent`

Health endpoint:

- `GET /health`

Documentation endpoints:

- `GET /openapi.json`
- `GET /docs`

## Query Parameters

- `location` (required for location-based endpoints): city name, postal code, or coordinates (`lat,long`)
- `continent` (required for continent endpoint)
- `start_date` and `end_date` (optional, must be provided together if used, format: `YYYY-MM-DD`)
- `weather_condition` (optional, only for `/weather/statistics`, values: `rain|snow|sunny|cloudy`)

## Date Default Behavior

If `start_date` and `end_date` are omitted, the API uses the full available range in the mock dataset.

## Sample Requests

```bash
curl -X GET "http://localhost:7010/weather/statistics?location=New%20York&start_date=2026-01-10&end_date=2026-01-11"
```

```bash
curl -X GET "http://localhost:7010/weather/statistics/average-temperature?location=40.7128,-74.0060&start_date=2026-01-10&end_date=2026-01-11"
```

```bash
curl -X GET "http://localhost:7010/weather/statistics/precipitation?location=10001&start_date=2026-01-10&end_date=2026-01-11"
```

```bash
curl -X GET "http://localhost:7010/weather/statistics/continent?continent=Europe&start_date=2026-01-10&end_date=2026-01-11"
```

## Test

```bash
npm test
```

## Notes

- Data source is in-memory mock data (`src/data/mockWeatherRepository.ts`).
- The service is layered: routes -> controllers -> validation/service -> data.
- Error contract format:

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "...",
    "details": "..."
  }
}
```
