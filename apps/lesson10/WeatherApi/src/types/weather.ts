export type WeatherCondition = "rain" | "snow" | "sunny" | "cloudy";

export interface WeatherObservation {
  city: string;
  country: string;
  continent: string;
  postalCode: string;
  latitude: number;
  longitude: number;
  isCapital: boolean;
  date: string;
  averageTemperature: number;
  precipitation: number;
  weatherCondition: WeatherCondition;
}

export interface DateRange {
  startDate: string;
  endDate: string;
}

export interface LocationQuery {
  kind: "city" | "postalCode" | "coordinates";
  value: string | { latitude: number; longitude: number };
}

export interface StatisticsQuery {
  location: LocationQuery;
  dateRange: DateRange;
  weatherCondition?: WeatherCondition;
}

export interface ContinentQuery {
  continent: string;
  dateRange: DateRange;
}

export interface WeatherStatisticsResponse {
  location: string;
  start_date: string;
  end_date: string;
  average_temperature: number;
  total_precipitation: number;
  weather_conditions: Record<string, number>;
}

export interface AverageTemperatureResponse {
  location: string;
  start_date: string;
  end_date: string;
  average_temperature: number;
}

export interface PrecipitationResponse {
  location: string;
  start_date: string;
  end_date: string;
  total_precipitation: number;
}

export interface ContinentStatisticsResponse {
  continent: string;
  start_date: string;
  end_date: string;
  capital_weather_conditions: Record<
    string,
    {
      average_temperature: number;
      total_precipitation: number;
      weather_conditions: Record<string, number>;
    }
  >;
}
