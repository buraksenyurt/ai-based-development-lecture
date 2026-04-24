import { createApp } from "./app";

const app = createApp();
const PORT = Number(process.env.PORT ?? "7010");

app.listen(PORT, () => {
  console.log(`Weather Statistic API is running on http://localhost:${PORT}`);
});
