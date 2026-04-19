using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var workspacePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Workspace"));
if (!Directory.Exists(workspacePath)) Directory.CreateDirectory(workspacePath);

/* 
    Mock verimiz.
    Şirkete özel benchmark sonuçları ve raporları temsil ettiğini düşünelim.
    Önemli nokta şudur; LLM bu verilere MCP olmadan erişemez!
*/
var benchmarkResults = new List<BenchmarkResult>
{
    new("LogParser_Zig", "Manual_Chunking", 100_000_000, 1420, 45.5, "v0.13.0"),
    new("LogParser_DotNet10", "Buffered_Read", 100_000_000, 1850, 120.2, "10.0.100"),
    new("LogParser_Rust", "Memory_Mapped", 100_000_000, 1380, 38.0, "1.78")
};

app.MapGet("/api/benchmarks", () => Results.Ok(benchmarkResults));

app.MapGet("/api/benchmarks/{name}", (string name) =>
{
    var result = benchmarkResults.FirstOrDefault(b => b.ProjectName.Equals(name, StringComparison.OrdinalIgnoreCase));
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

app.MapPost("/api/reports", async ([FromBody] ReportDto report) =>
{
    var fileName = $"{report.ReportName}_{DateTime.Now:yyyyMMdd_HHmm}.md";
    var filePath = Path.Combine(workspacePath, fileName);

    await File.WriteAllTextAsync(filePath, report.MarkdownContent);
    Console.WriteLine($"[AGENT ACTION] Yeni performans raporu oluşturuldu: {fileName}");

    return Results.Created($"/files/{fileName}", new { Path = filePath });
});

app.Run("http://localhost:5000");

public record BenchmarkResult(string ProjectName, string Method, long LinesProcessed, int ExecutionTimeMs, double MemoryUsageMb, string RuntimeVersion);
public record ReportDto(string ReportName, string MarkdownContent);