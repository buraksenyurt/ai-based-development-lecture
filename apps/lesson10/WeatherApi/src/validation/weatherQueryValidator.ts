import { getDatasetDateRange } from "../data/mockWeatherRepository";
import { ContinentQuery, DateRange, LocationQuery, StatisticsQuery, WeatherCondition } from "../types/weather";

const ISO_DATE_REGEX = /^\d{4}-\d{2}-\d{2}$/;
const COORDINATE_REGEX = /^\s*(-?\d{1,3}(?:\.\d+)?)\s*,\s*(-?\d{1,3}(?:\.\d+)?)\s*$/;
const VALID_CONDITIONS: WeatherCondition[] = ["rain", "snow", "sunny", "cloudy"];

export class ValidationError extends Error {
  constructor(public readonly details: string) {
    super(details);
    this.name = "ValidationError";
  }
}

function validateIsoDate(value: string, fieldName: string): void {
  if (!ISO_DATE_REGEX.test(value)) {
    throw new ValidationError(`${fieldName} must be in YYYY-MM-DD format`);
  }

  const date = new Date(`${value}T00:00:00Z`);
  if (Number.isNaN(date.getTime())) {
    throw new ValidationError(`${fieldName} is not a valid calendar date`);
  }
}

function resolveDateRange(startDate?: string, endDate?: string): DateRange {
  if (!startDate && !endDate) {
    const range = getDatasetDateRange();
    return {
      startDate: range.minDate,
      endDate: range.maxDate
    };
  }

  if (!startDate || !endDate) {
    throw new ValidationError("start_date and end_date must be provided together");
  }

  validateIsoDate(startDate, "start_date");
  validateIsoDate(endDate, "end_date");

  if (startDate > endDate) {
    throw new ValidationError("start_date cannot be greater than end_date");
  }

  return {
    startDate,
    endDate
  };
}

function normalizeLocation(rawLocation?: string): LocationQuery {
  if (!rawLocation || !rawLocation.trim()) {
    throw new ValidationError("location is required");
  }

  const location = rawLocation.trim();
  const coordinateMatch = location.match(COORDINATE_REGEX);

  if (coordinateMatch) {
    const latitude = Number(coordinateMatch[1]);
    const longitude = Number(coordinateMatch[2]);

    if (latitude < -90 || latitude > 90) {
      throw new ValidationError("latitude must be between -90 and 90");
    }

    if (longitude < -180 || longitude > 180) {
      throw new ValidationError("longitude must be between -180 and 180");
    }

    return {
      kind: "coordinates",
      value: { latitude, longitude }
    };
  }

  if (/^\d{4,10}$/.test(location)) {
    return {
      kind: "postalCode",
      value: location
    };
  }

  return {
    kind: "city",
    value: location.toLowerCase()
  };
}

export function validateStatisticsQuery(params: {
  location?: string;
  start_date?: string;
  end_date?: string;
  weather_condition?: string;
}): StatisticsQuery {
  const query: StatisticsQuery = {
    location: normalizeLocation(params.location),
    dateRange: resolveDateRange(params.start_date, params.end_date)
  };

  if (params.weather_condition) {
    const condition = params.weather_condition.trim().toLowerCase() as WeatherCondition;
    if (!VALID_CONDITIONS.includes(condition)) {
      throw new ValidationError("weather_condition must be one of: rain, snow, sunny, cloudy");
    }
    query.weatherCondition = condition;
  }

  return query;
}

export function validateContinentQuery(params: {
  continent?: string;
  start_date?: string;
  end_date?: string;
}): ContinentQuery {
  if (!params.continent || !params.continent.trim()) {
    throw new ValidationError("continent is required");
  }

  return {
    continent: params.continent.trim().toLowerCase(),
    dateRange: resolveDateRange(params.start_date, params.end_date)
  };
}
