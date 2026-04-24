import assert from "node:assert/strict";
import { describe, it } from "node:test";
import request from "supertest";
import { createApp } from "../src/app";

const app = createApp();

describe("weather routes", () => {
  it("GET /openapi.json returns OpenAPI document", async () => {
    const response = await request(app).get("/openapi.json");

    assert.equal(response.status, 200);
    assert.equal(response.body.openapi, "3.0.3");
    assert.equal(response.body.info.title, "Weather Statistic API");
  });

  it("GET /docs serves Swagger UI html", async () => {
    const response = await request(app).get("/docs");

    assert.equal(response.status, 301);
    assert.equal(response.headers.location, "/docs/");
  });

  it("GET /weather/statistics returns data", async () => {
    const response = await request(app)
      .get("/weather/statistics")
      .query({
        location: "New York",
        start_date: "2026-01-10",
        end_date: "2026-01-11"
      });

    assert.equal(response.status, 200);
    assert.equal(response.body.location, "new york");
    assert.equal(response.body.average_temperature, 5.2);
    assert.equal(response.body.total_precipitation, 4.3);
    assert.deepEqual(response.body.weather_conditions, {
      snow: 1,
      sunny: 1
    });
    assert.ok(response.body);
  });

  it("GET /weather/statistics validates missing location", async () => {
    const response = await request(app).get("/weather/statistics");

    assert.equal(response.status, 400);
    assert.equal(response.body.error.code, "VALIDATION_ERROR");
  });

  it("GET /weather/statistics/continent returns sorted capitals", async () => {
    const response = await request(app)
      .get("/weather/statistics/continent")
      .query({
        continent: "Europe",
        start_date: "2026-01-10",
        end_date: "2026-01-11"
      });

    assert.equal(response.status, 200);
    assert.deepEqual(Object.keys(response.body.capital_weather_conditions), ["London", "Paris"]);
  });

  it("GET unknown route returns 404", async () => {
    const response = await request(app).get("/weather/unknown");
    assert.equal(response.status, 404);
    assert.equal(response.body.error.code, "ROUTE_NOT_FOUND");
  });
});
