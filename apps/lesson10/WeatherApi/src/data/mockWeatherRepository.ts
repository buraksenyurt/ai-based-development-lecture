import { WeatherObservation } from "../types/weather";

const weatherData: WeatherObservation[] = [
  {
    city: "London",
    country: "United Kingdom",
    continent: "Europe",
    postalCode: "EC1A1BB",
    latitude: 51.5074,
    longitude: -0.1278,
    isCapital: true,
    date: "2026-01-10",
    averageTemperature: 7.5,
    precipitation: 4.2,
    weatherCondition: "rain"
  },
  {
    city: "London",
    country: "United Kingdom",
    continent: "Europe",
    postalCode: "EC1A1BB",
    latitude: 51.5074,
    longitude: -0.1278,
    isCapital: true,
    date: "2026-01-11",
    averageTemperature: 8.2,
    precipitation: 1.1,
    weatherCondition: "cloudy"
  },
  {
    city: "Paris",
    country: "France",
    continent: "Europe",
    postalCode: "75001",
    latitude: 48.8566,
    longitude: 2.3522,
    isCapital: true,
    date: "2026-01-10",
    averageTemperature: 8,
    precipitation: 2.4,
    weatherCondition: "rain"
  },
  {
    city: "Paris",
    country: "France",
    continent: "Europe",
    postalCode: "75001",
    latitude: 48.8566,
    longitude: 2.3522,
    isCapital: true,
    date: "2026-01-11",
    averageTemperature: 8.4,
    precipitation: 0,
    weatherCondition: "sunny"
  },
  {
    city: "Ankara",
    country: "Turkey",
    continent: "Asia",
    postalCode: "06000",
    latitude: 39.9334,
    longitude: 32.8597,
    isCapital: true,
    date: "2026-01-10",
    averageTemperature: 2.5,
    precipitation: 1.8,
    weatherCondition: "snow"
  },
  {
    city: "Ankara",
    country: "Turkey",
    continent: "Asia",
    postalCode: "06000",
    latitude: 39.9334,
    longitude: 32.8597,
    isCapital: true,
    date: "2026-01-11",
    averageTemperature: 3.8,
    precipitation: 0,
    weatherCondition: "sunny"
  },
  {
    city: "New York",
    country: "United States",
    continent: "North America",
    postalCode: "10001",
    latitude: 40.7128,
    longitude: -74.006,
    isCapital: false,
    date: "2026-01-10",
    averageTemperature: 4.8,
    precipitation: 3.6,
    weatherCondition: "snow"
  },
  {
    city: "New York",
    country: "United States",
    continent: "North America",
    postalCode: "10001",
    latitude: 40.7128,
    longitude: -74.006,
    isCapital: false,
    date: "2026-01-11",
    averageTemperature: 5.6,
    precipitation: 0.7,
    weatherCondition: "sunny"
  },
  {
    city: "Washington",
    country: "United States",
    continent: "North America",
    postalCode: "20001",
    latitude: 38.9072,
    longitude: -77.0369,
    isCapital: true,
    date: "2026-01-10",
    averageTemperature: 6.1,
    precipitation: 1.2,
    weatherCondition: "rain"
  },
  {
    city: "Washington",
    country: "United States",
    continent: "North America",
    postalCode: "20001",
    latitude: 38.9072,
    longitude: -77.0369,
    isCapital: true,
    date: "2026-01-11",
    averageTemperature: 7,
    precipitation: 0,
    weatherCondition: "cloudy"
  }
];

export function getAllObservations(): WeatherObservation[] {
  return weatherData;
}

export function getDatasetDateRange(): { minDate: string; maxDate: string } {
  const sortedDates = [...weatherData].map((item) => item.date).sort();
  return {
    minDate: sortedDates[0],
    maxDate: sortedDates[sortedDates.length - 1]
  };
}
