import { NextFunction, Request, Response } from "express";
import {
  getAverageTemperature,
  getContinentCapitalStatistics,
  getPrecipitation,
  getWeatherStatistics
} from "../services/weatherStatisticsService";
import { validateContinentQuery, validateStatisticsQuery } from "../validation/weatherQueryValidator";

export function weatherStatisticsController(req: Request, res: Response, next: NextFunction): void {
  try {
    const query = validateStatisticsQuery({
      location: req.query.location?.toString(),
      start_date: req.query.start_date?.toString(),
      end_date: req.query.end_date?.toString(),
      weather_condition: req.query.weather_condition?.toString()
    });

    const result = getWeatherStatistics(query);
    res.json(result);
  } catch (error) {
    next(error);
  }
}

export function averageTemperatureController(req: Request, res: Response, next: NextFunction): void {
  try {
    const query = validateStatisticsQuery({
      location: req.query.location?.toString(),
      start_date: req.query.start_date?.toString(),
      end_date: req.query.end_date?.toString()
    });

    const result = getAverageTemperature(query);
    res.json(result);
  } catch (error) {
    next(error);
  }
}

export function precipitationController(req: Request, res: Response, next: NextFunction): void {
  try {
    const query = validateStatisticsQuery({
      location: req.query.location?.toString(),
      start_date: req.query.start_date?.toString(),
      end_date: req.query.end_date?.toString()
    });

    const result = getPrecipitation(query);
    res.json(result);
  } catch (error) {
    next(error);
  }
}

export function continentStatisticsController(req: Request, res: Response, next: NextFunction): void {
  try {
    const query = validateContinentQuery({
      continent: req.query.continent?.toString(),
      start_date: req.query.start_date?.toString(),
      end_date: req.query.end_date?.toString()
    });

    const result = getContinentCapitalStatistics(query);
    res.json(result);
  } catch (error) {
    next(error);
  }
}
