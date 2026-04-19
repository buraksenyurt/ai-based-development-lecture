using System.Net.Http.Json;

namespace McpServerStdio.Services;

public class BenchmarkApiService(IHttpClientFactory httpClientFactory, string baseUrl)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly string _baseUrl = baseUrl.TrimEnd('/');

    public async Task<string> GetAllBenchmarksAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/api/benchmarks");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetBenchmarkByNameAsync(string name)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/api/benchmarks/{name}");

        if (!response.IsSuccessStatusCode)
            return $"Sistemde '{name}' adında bir benchmark kaydı bulunamadı.";

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> CreateReportAsync(string reportName, string markdownContent)
    {
        var payload = new { ReportName = reportName, MarkdownContent = markdownContent };
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{_baseUrl}/api/reports", payload);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}