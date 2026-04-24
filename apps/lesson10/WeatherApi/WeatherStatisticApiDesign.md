---
name: Weather Statistic API Design
description: This document describes the design of the Weather Statistic API, which provides endpoints for retrieving weather statistics based on various parameters.
---
# Weather Statistic API Design

## Overview

The Weather Statistic API is designed to provide users with access to weather statistics based on various parameters such as location, date range, and specific weather conditions. The API will allow users to retrieve aggregated weather data, including average temperatures, precipitation levels, and other relevant statistics.

## Endpoints

| **Endpoint** | **Method** | **Parameters (If Available)** | **Description** |
| --- | --- | --- | --- |
| `/weather/statistics` | GET | `location` (required), `start_date` (optional), `end_date` (optional), `weather_condition` (optional) | Retrieves weather statistics based on the specified parameters. |
| `/weather/statistics/average-temperature` | GET | `location` (required), `start_date` (optional), `end_date` (optional) | Retrieves the average temperature for the specified location and date range. |
| `/weather/statistics/precipitation` | GET | `location` (required), `start_date` (optional), `end_date` (optional) | Retrieves the total precipitation levels for the specified location and date range. |
| `/weather/statistics/continent` | GET | `continent` (required), `start_date` (optional), `end_date` (optional) | Retrieves the capital's weather conditions for the specified continent and date range by sorting city name |

## Parameters

- `location`: The geographical location for which the weather statistics are to be retrieved. This can be specified as a city name, postal code, or geographic coordinates (latitude and longitude). This parameter is required for the `/weather/statistics`, `/weather/statistics/average-temperature`, and `/weather/statistics/precipitation` endpoints.
- `continent`: The continent for which the capital's weather conditions are to be retrieved. This parameter is required for the `/weather/statistics/continent` endpoint.
- `start_date`: The start date for the date range of the weather statistics. This parameter is optional and defaults to the current date if not provided. This parameter should be in the format `YYYY-MM-DD`.
- `end_date`: The end date for the date range of the weather statistics. This parameter is optional and defaults to the current date if not provided. This parameter should be in the format `YYYY-MM-DD`.
- `weather_condition`: A specific weather condition (e.g., "rain", "snow", "sunny") for which the statistics are to be retrieved. This parameter is optional and can be used to filter the results based on specific weather conditions.

## Samples

### Get Weather Statistics by City Name

**Request:**

```bash
curl -X GET "https://api.weather.com/v1/weather/statistics?location=New%20York&start_date=2026-01-01&end_date=2026-01-31"
```

**Response:**

```json
{
  "location": "New York",
  "start_date": "2026-01-01",
  "end_date": "2026-01-31",
  "average_temperature": 5.2,
  "total_precipitation": 30.5,
  "weather_conditions": {
    "rain": 10,
    "snow": 5,
    "sunny": 16
  }
}
```

### Get Average Temperature by Geographic Coordinates

```bash
curl -X GET "https://api.weather.com/v1/weather/statistics/average-temperature?location=40.7128,-74.0060&start_date=2026-01-01&end_date=2026-01-31"
```

**Response:**

```json
{
  "location": "40.7128,-74.0060",
  "start_date": "2026-01-01",
  "end_date": "2026-01-31",
  "average_temperature": 5.2
}
```

### Get Precipitation Levels by Postal Code

```bash
curl -X GET "https://api.weather.com/v1/weather/statistics/precipitation?location=10001&start_date=2026-01-01&end_date=2026-01-31"
```

**Response:**

```json
{
  "location": "10001",
  "start_date": "2026-01-01",
  "end_date": "2026-01-31",
  "total_precipitation": 30.5
}
```

### Get Weather Conditions by Continent

```bash
curl -X GET "https://api.weather.com/v1/weather/statistics/continent?continent=Europe&start_date=2026-01-01&end_date=2026-01-31"
```

**Response:**

```json
{
  "continent": "Europe",
  "start_date": "2026-01-01",
  "end_date": "2026-01-31",
  "capital_weather_conditions": {
    "London": {
      "average_temperature": 7.5,
      "total_precipitation": 25.0,
      "weather_conditions": {
        "rain": 12,
        "snow": 3,
        "sunny": 16
      }
    },
    "Paris": {
      "average_temperature": 8.0,
      "total_precipitation": 20.0,
      "weather_conditions": {
        "rain": 10,
        "snow": 2,
        "sunny": 19
      }
    },
    // Additional capitals...
  }
}
```

## Conclusion

The Weather Statistic API provides a comprehensive set of endpoints for retrieving weather statistics based on various parameters. Users can access aggregated weather data for specific locations, date ranges, and weather conditions, allowing them to gain insights into historical weather patterns and trends.
