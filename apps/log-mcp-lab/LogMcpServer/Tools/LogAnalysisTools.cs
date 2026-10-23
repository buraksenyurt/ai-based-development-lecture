using System.ComponentModel;
using LogMcpServer.Models;
using LogMcpServer.Services;
using ModelContextProtocol.Server;

namespace LogMcpServer.Tools;

[McpServerToolType]
public static class LogAnalysisTools
{
    [McpServerTool, Description("Belirtilen zaman aralığında Loki'ye log gönderen hizmet adlarını (service_name) listeler. Kullanıcının bahsettiği hizmetin Loki'deki gerçek adını bulmak için diğer araçlardan önce bu aracı kullanın.")]
    public static async Task<string> ListServices(
        LokiClient lokiClient,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var services = await lokiClient.GetServiceNamesAsync(range.Start, range.End, cancellationToken);

        return ToolSupport.ToJson(new
        {
            rangeStart = range.Start,
            rangeEnd = range.End,
            serviceCount = services.Count,
            services
        });
    }

    [McpServerTool, Description("Bir hizmetteki eşik üstü response-time kayıtlarını beş dakikalık zaman pencerelerinde raporlar. Hizmet adı Loki'deki service_name değeridir; emin değilseniz önce list_services aracını kullanın.")]
    public static async Task<string> GetServiceBottleneckWindows(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri (örn. orders-api). Geçerli adlar için list_services aracını kullanın.")] string serviceName,
        [Description("Dar boğaz eşiği milisaniye cinsinden. Varsayılan: 500.")] double thresholdMs = 500,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        if (thresholdMs <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(thresholdMs), "thresholdMs must be greater than zero.");
        }

        var service = ToolSupport.NormalizeServiceName(serviceName)
            ?? throw new ArgumentException("serviceName is required.", nameof(serviceName));
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var entries = await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken);
        var windows = entries
            .Where(entry => entry.DurationMs > thresholdMs)
            .GroupBy(entry => new DateTimeOffset(
                entry.Timestamp.Year,
                entry.Timestamp.Month,
                entry.Timestamp.Day,
                entry.Timestamp.Hour,
                (entry.Timestamp.Minute / 5) * 5,
                0,
                TimeSpan.Zero))
            .Select(group => new
            {
                windowStart = group.Key,
                windowEnd = group.Key.AddMinutes(5),
                eventCount = group.Count(),
                averageDurationMs = Math.Round(group.Average(entry => entry.DurationMs!.Value), 2),
                maxDurationMs = group.Max(entry => entry.DurationMs!.Value)
            })
            .ToArray();

        return ToolSupport.ToJson(new
        {
            service,
            thresholdMs,
            rangeStart = range.Start,
            rangeEnd = range.End,
            bottleneckWindows = windows
        });
    }

    [McpServerTool, Description("Loki loglarından ErrorBurstDetected ve SustainedLatencyDetected alarm koşullarını hizmet bazında raporlar.")]
    public static async Task<string> GetAlarmMetrics(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri. Boş bırakılırsa tüm hizmetler sorgulanır.")] string? serviceName = null,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var service = ToolSupport.NormalizeServiceName(serviceName);
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var entries = await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken);
        var metrics = entries
            .GroupBy(entry => entry.ServiceName)
            .Select(group =>
            {
                var errorEntries = group.Where(entry => entry.Severity == "ERROR").ToArray();
                var slowEntries = group.Where(entry => entry.DurationMs > 500).ToArray();
                var maximumErrorsInWindow = GetMaximumCountInFiveMinuteWindow(errorEntries);
                var maximumSlowResponsesInWindow = GetMaximumCountInFiveMinuteWindow(slowEntries);

                return new
                {
                    service = group.Key,
                    errorCount = errorEntries.Length,
                    slowResponseCount = slowEntries.Length,
                    maximumErrorsInFiveMinuteWindow = maximumErrorsInWindow,
                    maximumSlowResponsesInFiveMinuteWindow = maximumSlowResponsesInWindow,
                    errorBurstThresholdExceeded = maximumErrorsInWindow > 5,
                    latencyThresholdExceeded = maximumSlowResponsesInWindow > 5
                };
            })
            .OrderByDescending(metric => metric.errorCount)
            .ThenByDescending(metric => metric.slowResponseCount)
            .ToArray();

        return ToolSupport.ToJson(new
        {
            service,
            rangeStart = range.Start,
            rangeEnd = range.End,
            errorBurstThreshold = 5,
            latencyThresholdMs = 500,
            metrics
        });
    }

    [McpServerTool, Description("Bir hizmetin response-time kayıtlarından minimum, ortalama ve maksimum süreyi raporlar. Hizmet adı Loki'deki service_name değeridir; emin değilseniz önce list_services aracını kullanın.")]
    public static async Task<string> GetResponseTimeSummary(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri (örn. orders-api). Geçerli adlar için list_services aracını kullanın.")] string serviceName,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var service = ToolSupport.NormalizeServiceName(serviceName)
            ?? throw new ArgumentException("serviceName is required.", nameof(serviceName));
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var durations = (await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken))
            .Where(entry => entry.DurationMs.HasValue)
            .Select(entry => entry.DurationMs!.Value)
            .ToArray();

        return ToolSupport.ToJson(new
        {
            service,
            rangeStart = range.Start,
            rangeEnd = range.End,
            sampleCount = durations.Length,
            minimumDurationMs = durations.Length == 0 ? (double?)null : durations.Min(),
            averageDurationMs = durations.Length == 0 ? (double?)null : Math.Round(durations.Average(), 2),
            maximumDurationMs = durations.Length == 0 ? (double?)null : durations.Max()
        });
    }

    [McpServerTool, Description("ERROR loglarını hizmet ve hata türüne göre özetleyerek hata patlamalarını gösterir.")]
    public static async Task<string> GetErrorBurstSummary(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri. Boş bırakılırsa tüm hizmetler sorgulanır.")] string? serviceName = null,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var service = ToolSupport.NormalizeServiceName(serviceName);
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var errors = (await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken))
            .Where(entry => entry.Severity == "ERROR")
            .GroupBy(entry => new { entry.ServiceName, ErrorType = entry.ErrorType ?? "unspecified" })
            .Select(group => new
            {
                service = group.Key.ServiceName,
                errorType = group.Key.ErrorType,
                count = group.Count(),
                firstSeen = group.Min(entry => entry.Timestamp),
                lastSeen = group.Max(entry => entry.Timestamp)
            })
            .OrderByDescending(summary => summary.count)
            .ToArray();

        return ToolSupport.ToJson(new { service, rangeStart = range.Start, rangeEnd = range.End, errors });
    }

    [McpServerTool, Description("dependency_failure senaryosundaki logları bağımlı hizmet ve HTTP durumuna göre özetler.")]
    public static async Task<string> GetDependencyFailureSummary(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri. Boş bırakılırsa tüm hizmetler sorgulanır.")] string? serviceName = null,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var service = ToolSupport.NormalizeServiceName(serviceName);
        var range = ToolSupport.ResolveRange(startTime, endTime);
        var failures = (await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken))
            .Where(entry => entry.Scenario == "dependency_failure")
            .GroupBy(entry => new
            {
                entry.ServiceName,
                Dependency = entry.DependencyName ?? "unknown",
                StatusCode = entry.HttpStatusCode
            })
            .Select(group => new
            {
                service = group.Key.ServiceName,
                dependency = group.Key.Dependency,
                httpStatusCode = group.Key.StatusCode,
                count = group.Count(),
                firstSeen = group.Min(entry => entry.Timestamp),
                lastSeen = group.Max(entry => entry.Timestamp)
            })
            .OrderByDescending(summary => summary.count)
            .ToArray();

        return ToolSupport.ToJson(new { rangeStart = range.Start, rangeEnd = range.End, failures });
    }

    [McpServerTool, Description("Hizmet, önem seviyesi ve olay adı filtreleriyle sınırlı sayıda ham log örneğini getirir.")]
    public static async Task<string> SearchLogs(
        LokiClient lokiClient,
        [Description("Loki'deki service_name değeri. Boş bırakılırsa tüm hizmetler sorgulanır.")] string? serviceName = null,
        [Description("DEBUG, INFO, WARN, ERROR veya FATAL. Boş bırakılırsa tüm önem seviyeleri gelir.")] string? severity = null,
        [Description("Tam olay adı. Boş bırakılırsa tüm olaylar gelir.")] string? eventName = null,
        [Description("Dönecek en fazla kayıt sayısı. 1-100 arası, varsayılan: 50.")] int limit = 50,
        [Description("RFC 3339 UTC başlangıç zamanı. Boşsa son 24 saat kullanılır.")] string? startTime = null,
        [Description("RFC 3339 UTC bitiş zamanı. Boşsa son 24 saat kullanılır.")] string? endTime = null,
        CancellationToken cancellationToken = default)
    {
        var service = ToolSupport.NormalizeServiceName(serviceName);
        var normalizedSeverity = severity is null ? null : ToolSupport.NormalizeSeverity(severity);
        if (eventName is not null && !System.Text.RegularExpressions.Regex.IsMatch(eventName, "^[A-Za-z0-9._-]+$"))
        {
            throw new ArgumentException("eventName may contain only letters, digits, dots, underscores, and hyphens.");
        }

        var range = ToolSupport.ResolveRange(startTime, endTime);
        var entries = (await lokiClient.QueryLogsAsync(service, range.Start, range.End, cancellationToken))
            .Where(entry => normalizedSeverity is null || entry.Severity == normalizedSeverity)
            .Where(entry => eventName is null || entry.EventName == eventName)
            .Take(ToolSupport.NormalizeLimit(limit))
            .ToArray();

        return ToolSupport.ToJson(new
        {
            service,
            severity = normalizedSeverity,
            eventName,
            rangeStart = range.Start,
            rangeEnd = range.End,
            returnedCount = entries.Length,
            entries
        });
    }

    private static int GetMaximumCountInFiveMinuteWindow(IEnumerable<LokiLogEntry> entries) =>
        entries
            .GroupBy(entry => new DateTimeOffset(
                entry.Timestamp.Year,
                entry.Timestamp.Month,
                entry.Timestamp.Day,
                entry.Timestamp.Hour,
                (entry.Timestamp.Minute / 5) * 5,
                0,
                TimeSpan.Zero))
            .Select(group => group.Count())
            .DefaultIfEmpty()
            .Max();
}
