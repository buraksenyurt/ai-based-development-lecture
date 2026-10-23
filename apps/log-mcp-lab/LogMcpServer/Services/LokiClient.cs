using System.Globalization;
using System.Text.Json;
using LogMcpServer.Models;

namespace LogMcpServer.Services;

public sealed class LokiClient(HttpClient httpClient, LokiOptions options)
{
    public async Task<IReadOnlyList<LokiLogEntry>> QueryLogsAsync(
        string? serviceName,
        DateTimeOffset start,
        DateTimeOffset end,
        CancellationToken cancellationToken)
    {
        var selector = serviceName is null
            ? "{job=\"synthetic-lab\"}"
            : $"{{job=\"synthetic-lab\",service_name=\"{serviceName}\"}}";
        var query = $"{selector} | json";
        var requestUri =
            $"/loki/api/v1/query_range?query={Uri.EscapeDataString(query)}&start={ToNanoseconds(start)}&end={ToNanoseconds(end)}&limit={options.MaximumResultCount}";

        using var response = await httpClient.GetAsync(requestUri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(responseStream, cancellationToken: cancellationToken);
        return ParseEntries(document.RootElement);
    }

    public async Task<IReadOnlyList<string>> GetServiceNamesAsync(
        DateTimeOffset start,
        DateTimeOffset end,
        CancellationToken cancellationToken)
    {
        const string selector = "{job=\"synthetic-lab\"}";
        var requestUri =
            $"/loki/api/v1/label/service_name/values?query={Uri.EscapeDataString(selector)}&start={ToNanoseconds(start)}&end={ToNanoseconds(end)}";

        using var response = await httpClient.GetAsync(requestUri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var document = await JsonDocument.ParseAsync(responseStream, cancellationToken: cancellationToken);

        // Aralıkta hiç kayıt yoksa Loki "data" alanını boş/null döndürebilir.
        if (!document.RootElement.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return data.EnumerateArray()
            .Where(value => value.ValueKind == JsonValueKind.String)
            .Select(value => value.GetString()!)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
    }

    private static long ToNanoseconds(DateTimeOffset value) => value.ToUnixTimeMilliseconds() * 1_000_000;

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException(
            $"Loki query failed with status {(int)response.StatusCode}: {responseBody[..Math.Min(responseBody.Length, 500)]}");
    }

    private static IReadOnlyList<LokiLogEntry> ParseEntries(JsonElement root)
    {
        var entries = new List<LokiLogEntry>();
        if (!root.TryGetProperty("data", out var data) ||
            !data.TryGetProperty("result", out var result) ||
            result.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("Loki response did not contain a log result array.");
        }

        foreach (var stream in result.EnumerateArray())
        {
            if (!stream.TryGetProperty("values", out var values) || values.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var value in values.EnumerateArray())
            {
                if (value.ValueKind != JsonValueKind.Array || value.GetArrayLength() != 2)
                {
                    continue;
                }

                var timestampNanoseconds = long.Parse(value[0].GetString()!, CultureInfo.InvariantCulture);
                var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestampNanoseconds / 1_000_000);
                var line = value[1].GetString() ?? throw new InvalidOperationException("Loki log line was empty.");
                entries.Add(ParseLogLine(timestamp, line));
            }
        }

        return entries.OrderBy(entry => entry.Timestamp).ToArray();
    }

    private static LokiLogEntry ParseLogLine(DateTimeOffset timestamp, string line)
    {
        using var document = JsonDocument.Parse(line);
        var root = document.RootElement;
        var attributes = root.TryGetProperty("attributes", out var attributeElement)
            ? attributeElement
            : default;

        return new LokiLogEntry(
            timestamp,
            GetString(root, "service_name") ?? "unknown",
            GetString(root, "severity_text") ?? "UNKNOWN",
            GetString(root, "event_name") ?? "unknown",
            GetString(root, "body") ?? string.Empty,
            GetDouble(attributes, "duration_ms"),
            GetString(attributes, "error.type"),
            GetString(attributes, "dependency.name"),
            GetInt32(attributes, "http.response.status_code"),
            GetString(attributes, "scenario"));
    }

    private static string? GetString(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;

    private static double? GetDouble(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.TryGetDouble(out var value)
            ? value
            : null;

    private static int? GetInt32(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var property) &&
        property.TryGetInt32(out var value)
            ? value
            : null;
}
