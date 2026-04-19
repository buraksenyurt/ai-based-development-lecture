using McpServerSSE.Services;
using ModelContextProtocol.Protocol;

/*
    Örnek promptlar:

    - "Sistemimizde şu an hangi log parser benchmark projeleri koşuyor? Sadece isimlerini listele."
    - "Zig ve Rust tabanlı log parser projelerinin detaylı metriklerini çek. Hangisinin yürütme süresi (ExecutionTime) daha kısa, hangisinin bellek tüketimi (MemoryUsage) daha az? Bu analizi Markdown formatında güzel bir tablo haline getir ve sisteme 'zig_vs_rust_analizi' adıyla rapor olarak kaydet."

*/

var builder = WebApplication.CreateBuilder(args);

string apiUrl = "http://localhost:5000/";

builder.Services.AddHttpClient();
builder.Services.AddSingleton(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new BenchmarkApiService(httpClientFactory, apiUrl);
});

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new Implementation
        {
            Name = "BenchmarkServer_SSE",
            Version = "1.0.0"
        };
    })
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.MapMcp("/sse");

Console.WriteLine("[MCP SERVER INFO] BenchmarkServer_SSE başlatılıyor...");
Console.WriteLine($"[MCP SERVER INFO] Hedef API: {apiUrl}");
Console.WriteLine("[MCP SERVER INFO] SSE endpoint: /sse");

app.Run();
