import { NextFunction, Request, Response } from "express";
import { NotFoundError } from "../services/weatherStatisticsService";
import { ValidationError } from "../validation/weatherQueryValidator";

export function notFoundHandler(_req: Request, res: Response): void {
  res.status(404).json({
    error: {
      code: "ROUTE_NOT_FOUND",
      message: "Requested route does not exist"
    }
  });
}

export function errorHandler(error: Error, _req: Request, res: Response, _next: NextFunction): void {
  if (error instanceof ValidationError) {
    res.status(400).json({
      error: {
        code: "VALIDATION_ERROR",
        message: error.message,
        details: error.details
      }
    });
    return;
  }

  if (error instanceof NotFoundError) {
    res.status(404).json({
      error: {
        code: "NOT_FOUND",
        message: error.message
      }
    });
    return;
  }

  res.status(500).json({
    error: {
      code: "INTERNAL_ERROR",
      message: "Unexpected server error"
    }
  });
}
