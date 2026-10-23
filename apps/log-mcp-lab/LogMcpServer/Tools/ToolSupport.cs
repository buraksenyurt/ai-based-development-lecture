using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LogMcpServer.Tools;

internal static partial class ToolSupport
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static (DateTimeOffset Start, DateTimeOffset End) ResolveRange(string? startTime, string? endTime)
    {
        if (string.IsNullOrWhiteSpace(startTime) && string.IsNullOrWhiteSpace(endTime))
        {
            var defaultEnd = DateTimeOffset.UtcNow;
            return (defaultEnd.AddHours(-24), defaultEnd);
        }

        if (string.IsNullOrWhiteSpace(startTime) || string.IsNullOrWhiteSpace(endTime))
        {
            throw new ArgumentException("startTime and endTime must be supplied together as RFC 3339 UTC values.");
        }

        if (!DateTimeOffset.TryParse(
                startTime,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var start) ||
            !DateTimeOffset.TryParse(
                endTime,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var end))
        {
            throw new ArgumentException("startTime and endTime must be valid RFC 3339 timestamps.");
        }

        if (start >= end)
        {
            throw new ArgumentException("startTime must be earlier than endTime.");
        }

        return (start, end);
    }

    public static string? NormalizeServiceName(string? serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return null;
        }

        var normalized = serviceName.Trim();
        if (!ServiceNamePattern().IsMatch(normalized))
        {
            throw new ArgumentException("serviceName may contain only letters, digits, dots, underscores, and hyphens.");
        }

        return normalized;
    }

    public static string NormalizeSeverity(string? severity)
    {
        var normalized = (severity ?? string.Empty).Trim().ToUpperInvariant();
        if (normalized is not ("DEBUG" or "INFO" or "WARN" or "ERROR" or "FATAL"))
        {
            throw new ArgumentException("severity must be DEBUG, INFO, WARN, ERROR, or FATAL.");
        }

        return normalized;
    }

    public static int NormalizeLimit(int limit)
    {
        if (limit is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "limit must be between 1 and 100.");
        }

        return limit;
    }

    public static string ToJson(object value) => JsonSerializer.Serialize(value, JsonOptions);

    [GeneratedRegex("^[A-Za-z0-9._-]+$")]
    private static partial Regex ServiceNamePattern();
}
