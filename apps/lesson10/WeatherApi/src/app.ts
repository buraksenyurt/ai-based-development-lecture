import express, { Express } from "express";
import swaggerUi from "swagger-ui-express";
import { openApiDocument } from "./docs/openapi";
import { errorHandler, notFoundHandler } from "./middleware/errorHandler";
import { weatherRouter } from "./routes/weatherRoutes";

export function createApp(): Express {
  const app = express();

  app.use(express.json());
  app.use("/weather", weatherRouter);
  app.get("/openapi.json", (_req, res) => {
    res.json(openApiDocument);
  });
  app.use("/docs", swaggerUi.serve, swaggerUi.setup(openApiDocument));

  app.get("/health", (_req, res) => {
    res.json({ status: "ok" });
  });

  app.use(notFoundHandler);
  app.use(errorHandler);

  return app;
}
