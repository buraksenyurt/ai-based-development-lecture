import assert from "node:assert/strict";
import { describe, it } from "node:test";
import {
  getAverageTemperature,
  getContinentCapitalStatistics,
  getPrecipitation,
  getWeatherStatistics,
  NotFoundError
} from "../src/services/weatherStatisticsService";
import { validateContinentQuery, validateStatisticsQuery } from "../src/validation/weatherQueryValidator";

describe("weatherStatisticsService", () => {
  it("returns aggregated weather statistics by city", () => {
    const query = validateStatisticsQuery({
      location: "New York",
      start_date: "2026-01-10",
      end_date: "2026-01-11"
    });

    const result = getWeatherStatistics(query);

    assert.equal(result.location, "new york");
    assert.equal(result.average_temperature, 5.2);
    assert.equal(result.total_precipitation, 4.3);
    assert.deepEqual(result.weather_conditions, { snow: 1, sunny: 1 });
  });

  it("returns average temperature by coordinates", () => {
    const query = validateStatisticsQuery({
      location: "40.7128,-74.0060",
      start_date: "2026-01-10",
      end_date: "2026-01-11"
    });

    const result = getAverageTemperature(query);
    assert.equal(result.average_temperature, 5.2);
  });

  it("returns total precipitation by postal code", () => {
    const query = validateStatisticsQuery({
      location: "10001",
      start_date: "2026-01-10",
      end_date: "2026-01-11"
    });

    const result = getPrecipitation(query);
    assert.equal(result.total_precipitation, 4.3);
  });

  it("returns sorted continent capital statistics", () => {
    const query = validateContinentQuery({
      continent: "Europe",
      start_date: "2026-01-10",
      end_date: "2026-01-11"
    });

    const result = getContinentCapitalStatistics(query);
    assert.deepEqual(Object.keys(result.capital_weather_conditions), ["London", "Paris"]);
  });

  it("uses full dataset date range when dates are omitted", () => {
    const query = validateStatisticsQuery({
      location: "Ankara"
    });

    assert.deepEqual(query.dateRange, {
      startDate: "2026-01-10",
      endDate: "2026-01-11"
    });
  });

  it("throws not found when filters match no records", () => {
    const query = validateStatisticsQuery({
      location: "Berlin",
      start_date: "2026-01-10",
      end_date: "2026-01-11"
    });

    assert.throws(() => getWeatherStatistics(query), NotFoundError);
  });
});
