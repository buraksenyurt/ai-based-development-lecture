using McpServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/*
    Örnek promptlar:

    - "Sistemimizde şu an hangi log parser benchmark projeleri koşuyor? Sadece isimlerini listele."
    - "Zig ve Rust tabanlı log parser projelerinin detaylı metriklerini çek. Hangisinin yürütme süresi (ExecutionTime) daha kısa, hangisinin bellek tüketimi (MemoryUsage) daha az? Bu analizi Markdown formatında güzel bir tablo haline getir ve sisteme 'zig_vs_rust_analizi' adıyla rapor olarak kaydet."

*/

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

string apiUrl = "http://localhost:5000/";

builder.Services.AddHttpClient();
builder.Services.AddSingleton(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new BenchmarkApiService(httpClientFactory, apiUrl);
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();