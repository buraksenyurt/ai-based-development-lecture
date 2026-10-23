namespace LogMcpServer.Models;

public sealed record LokiLogEntry(
    DateTimeOffset Timestamp,
    string ServiceName,
    string Severity,
    string EventName,
    string Body,
    double? DurationMs,
    string? ErrorType,
    string? DependencyName,
    int? HttpStatusCode,
    string? Scenario);
