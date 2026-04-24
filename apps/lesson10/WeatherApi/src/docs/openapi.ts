export const openApiDocument = {
  openapi: "3.0.3",
  info: {
    title: "Weather Statistic API",
    version: "1.0.0",
    description: "REST API for retrieving weather statistics by location, date range, and continent."
  },
  servers: [
    {
      url: "http://localhost:7010"
    }
  ],
  tags: [
    { name: "Health" },
    { name: "Weather" }
  ],
  components: {
    parameters: {
      location: {
        name: "location",
        in: "query",
        required: true,
        description: "City name, postal code, or coordinates in lat,long format.",
        schema: { type: "string" }
      },
      continent: {
        name: "continent",
        in: "query",
        required: true,
        description: "Continent name.",
        schema: { type: "string", example: "Europe" }
      },
      startDate: {
        name: "start_date",
        in: "query",
        required: false,
        schema: { type: "string", format: "date", example: "2026-01-10" }
      },
      endDate: {
        name: "end_date",
        in: "query",
        required: false,
        schema: { type: "string", format: "date", example: "2026-01-11" }
      },
      weatherCondition: {
        name: "weather_condition",
        in: "query",
        required: false,
        schema: {
          type: "string",
          enum: ["rain", "snow", "sunny", "cloudy"]
        }
      }
    },
    schemas: {
      ErrorResponse: {
        type: "object",
        properties: {
          error: {
            type: "object",
            properties: {
              code: { type: "string" },
              message: { type: "string" },
              details: { type: "string" }
            },
            required: ["code", "message"]
          }
        },
        required: ["error"]
      },
      WeatherStatisticsResponse: {
        type: "object",
        properties: {
          location: { type: "string" },
          start_date: { type: "string", format: "date" },
          end_date: { type: "string", format: "date" },
          average_temperature: { type: "number" },
          total_precipitation: { type: "number" },
          weather_conditions: {
            type: "object",
            additionalProperties: { type: "number" }
          }
        },
        required: [
          "location",
          "start_date",
          "end_date",
          "average_temperature",
          "total_precipitation",
          "weather_conditions"
        ]
      },
      AverageTemperatureResponse: {
        type: "object",
        properties: {
          location: { type: "string" },
          start_date: { type: "string", format: "date" },
          end_date: { type: "string", format: "date" },
          average_temperature: { type: "number" }
        },
        required: ["location", "start_date", "end_date", "average_temperature"]
      },
      PrecipitationResponse: {
        type: "object",
        properties: {
          location: { type: "string" },
          start_date: { type: "string", format: "date" },
          end_date: { type: "string", format: "date" },
          total_precipitation: { type: "number" }
        },
        required: ["location", "start_date", "end_date", "total_precipitation"]
      },
      ContinentStatisticsResponse: {
        type: "object",
        properties: {
          continent: { type: "string" },
          start_date: { type: "string", format: "date" },
          end_date: { type: "string", format: "date" },
          capital_weather_conditions: {
            type: "object",
            additionalProperties: {
              type: "object",
              properties: {
                average_temperature: { type: "number" },
                total_precipitation: { type: "number" },
                weather_conditions: {
                  type: "object",
                  additionalProperties: { type: "number" }
                }
              },
              required: ["average_temperature", "total_precipitation", "weather_conditions"]
            }
          }
        },
        required: ["continent", "start_date", "end_date", "capital_weather_conditions"]
      }
    }
  },
  paths: {
    "/health": {
      get: {
        tags: ["Health"],
        summary: "Health check",
        responses: {
          "200": {
            description: "Service is healthy",
            content: {
              "application/json": {
                schema: {
                  type: "object",
                  properties: {
                    status: { type: "string", example: "ok" }
                  },
                  required: ["status"]
                }
              }
            }
          }
        }
      }
    },
    "/weather/statistics": {
      get: {
        tags: ["Weather"],
        summary: "Get weather statistics",
        parameters: [
          { "$ref": "#/components/parameters/location" },
          { "$ref": "#/components/parameters/startDate" },
          { "$ref": "#/components/parameters/endDate" },
          { "$ref": "#/components/parameters/weatherCondition" }
        ],
        responses: {
          "200": {
            description: "Aggregated weather statistics",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/WeatherStatisticsResponse" }
              }
            }
          },
          "400": {
            description: "Invalid query parameters",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          },
          "404": {
            description: "No matching data found",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          }
        }
      }
    },
    "/weather/statistics/average-temperature": {
      get: {
        tags: ["Weather"],
        summary: "Get average temperature",
        parameters: [
          { "$ref": "#/components/parameters/location" },
          { "$ref": "#/components/parameters/startDate" },
          { "$ref": "#/components/parameters/endDate" }
        ],
        responses: {
          "200": {
            description: "Average temperature result",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/AverageTemperatureResponse" }
              }
            }
          },
          "400": {
            description: "Invalid query parameters",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          },
          "404": {
            description: "No matching data found",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          }
        }
      }
    },
    "/weather/statistics/precipitation": {
      get: {
        tags: ["Weather"],
        summary: "Get total precipitation",
        parameters: [
          { "$ref": "#/components/parameters/location" },
          { "$ref": "#/components/parameters/startDate" },
          { "$ref": "#/components/parameters/endDate" }
        ],
        responses: {
          "200": {
            description: "Total precipitation result",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/PrecipitationResponse" }
              }
            }
          },
          "400": {
            description: "Invalid query parameters",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          },
          "404": {
            description: "No matching data found",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          }
        }
      }
    },
    "/weather/statistics/continent": {
      get: {
        tags: ["Weather"],
        summary: "Get capital weather statistics by continent",
        parameters: [
          { "$ref": "#/components/parameters/continent" },
          { "$ref": "#/components/parameters/startDate" },
          { "$ref": "#/components/parameters/endDate" }
        ],
        responses: {
          "200": {
            description: "Continent capital statistics",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ContinentStatisticsResponse" }
              }
            }
          },
          "400": {
            description: "Invalid query parameters",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          },
          "404": {
            description: "No matching data found",
            content: {
              "application/json": {
                schema: { "$ref": "#/components/schemas/ErrorResponse" }
              }
            }
          }
        }
      }
    }
  }
} as const;
