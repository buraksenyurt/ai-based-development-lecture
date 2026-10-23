namespace LogMcpServer.Services;

public sealed record LokiOptions(Uri BaseUri, TimeSpan RequestTimeout, int MaximumResultCount);
