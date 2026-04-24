import { Router } from "express";
import {
  averageTemperatureController,
  continentStatisticsController,
  precipitationController,
  weatherStatisticsController
} from "../controllers/weatherController";

const weatherRouter = Router();

weatherRouter.get("/statistics", weatherStatisticsController);
weatherRouter.get("/statistics/average-temperature", averageTemperatureController);
weatherRouter.get("/statistics/precipitation", precipitationController);
weatherRouter.get("/statistics/continent", continentStatisticsController);

export { weatherRouter };
