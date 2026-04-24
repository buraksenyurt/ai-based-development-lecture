import { getAllObservations } from "../data/mockWeatherRepository";
import {
  AverageTemperatureResponse,
  ContinentQuery,
  ContinentStatisticsResponse,
  LocationQuery,
  PrecipitationResponse,
  StatisticsQuery,
  WeatherObservation,
  WeatherStatisticsResponse
} from "../types/weather";

export class NotFoundError extends Error {
  constructor(message: string) {
    super(message);
    this.name = "NotFoundError";
  }
}

function roundToOneDigit(value: number): number {
  return Number(value.toFixed(1));
}

function isInDateRange(date: string, startDate: string, endDate: string): boolean {
  return date >= startDate && date <= endDate;
}

function matchesLocation(observation: WeatherObservation, location: LocationQuery): boolean {
  switch (location.kind) {
    case "city":
      return observation.city.toLowerCase() === location.value;
    case "postalCode":
      return observation.postalCode === location.value;
    case "coordinates": {
      const coordinates = location.value;
      if (typeof coordinates === "string") {
        return false;
      }

      const tolerance = 0.0001;
      return (
        Math.abs(observation.latitude - coordinates.latitude) <= tolerance &&
        Math.abs(observation.longitude - coordinates.longitude) <= tolerance
      );
    }
    default:
      return false;
  }
}

function computeConditionCounts(records: WeatherObservation[]): Record<string, number> {
  return records.reduce<Record<string, number>>((acc, item) => {
    acc[item.weatherCondition] = (acc[item.weatherCondition] ?? 0) + 1;
    return acc;
  }, {});
}

function averageTemperature(records: WeatherObservation[]): number {
  const sum = records.reduce((acc, item) => acc + item.averageTemperature, 0);
  return roundToOneDigit(sum / records.length);
}

function totalPrecipitation(records: WeatherObservation[]): number {
  const sum = records.reduce((acc, item) => acc + item.precipitation, 0);
  return roundToOneDigit(sum);
}

function formatLocation(location: LocationQuery): string {
  if (location.kind === "coordinates" && typeof location.value !== "string") {
    return `${location.value.latitude},${location.value.longitude}`;
  }

  return String(location.value);
}

function filterByLocationAndDate(query: StatisticsQuery): WeatherObservation[] {
  const filtered = getAllObservations().filter(
    (item) =>
      isInDateRange(item.date, query.dateRange.startDate, query.dateRange.endDate) &&
      matchesLocation(item, query.location)
  );

  if (query.weatherCondition) {
    return filtered.filter((item) => item.weatherCondition === query.weatherCondition);
  }

  return filtered;
}

export function getWeatherStatistics(query: StatisticsQuery): WeatherStatisticsResponse {
  const records = filterByLocationAndDate(query);

  if (!records.length) {
    throw new NotFoundError("No weather statistics found for the given filters");
  }

  return {
    location: formatLocation(query.location),
    start_date: query.dateRange.startDate,
    end_date: query.dateRange.endDate,
    average_temperature: averageTemperature(records),
    total_precipitation: totalPrecipitation(records),
    weather_conditions: computeConditionCounts(records)
  };
}

export function getAverageTemperature(query: StatisticsQuery): AverageTemperatureResponse {
  const records = filterByLocationAndDate(query);

  if (!records.length) {
    throw new NotFoundError("No weather observations found for average temperature");
  }

  return {
    location: formatLocation(query.location),
    start_date: query.dateRange.startDate,
    end_date: query.dateRange.endDate,
    average_temperature: averageTemperature(records)
  };
}

export function getPrecipitation(query: StatisticsQuery): PrecipitationResponse {
  const records = filterByLocationAndDate(query);

  if (!records.length) {
    throw new NotFoundError("No weather observations found for precipitation");
  }

  return {
    location: formatLocation(query.location),
    start_date: query.dateRange.startDate,
    end_date: query.dateRange.endDate,
    total_precipitation: totalPrecipitation(records)
  };
}

export function getContinentCapitalStatistics(query: ContinentQuery): ContinentStatisticsResponse {
  const records = getAllObservations().filter(
    (item) =>
      item.continent.toLowerCase() === query.continent &&
      item.isCapital &&
      isInDateRange(item.date, query.dateRange.startDate, query.dateRange.endDate)
  );

  if (!records.length) {
    throw new NotFoundError("No capital weather statistics found for this continent");
  }

  const cityMap = new Map<string, WeatherObservation[]>();
  for (const record of records) {
    const existing = cityMap.get(record.city) ?? [];
    existing.push(record);
    cityMap.set(record.city, existing);
  }

  const sortedCities = [...cityMap.keys()].sort((a, b) => a.localeCompare(b));
  const capital_weather_conditions = sortedCities.reduce<ContinentStatisticsResponse["capital_weather_conditions"]>(
    (acc, city) => {
      const cityRecords = cityMap.get(city) ?? [];
      acc[city] = {
        average_temperature: averageTemperature(cityRecords),
        total_precipitation: totalPrecipitation(cityRecords),
        weather_conditions: computeConditionCounts(cityRecords)
      };
      return acc;
    },
    {}
  );

  return {
    continent: query.continent,
    start_date: query.dateRange.startDate,
    end_date: query.dateRange.endDate,
    capital_weather_conditions
  };
}
