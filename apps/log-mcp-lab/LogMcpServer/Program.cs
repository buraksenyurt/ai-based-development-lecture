using LogMcpServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace);

var lokiBaseUrl = builder.Configuration["Loki:BaseUrl"] ?? "http://localhost:3101";
if (!Uri.TryCreate(lokiBaseUrl, UriKind.Absolute, out var lokiUri))
{
    throw new InvalidOperationException("Loki:BaseUrl must be an absolute URI.");
}

builder.Services.AddSingleton(new LokiOptions(lokiUri, TimeSpan.FromSeconds(30), 2_000));
builder.Services.AddHttpClient<LokiClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<LokiOptions>();
    client.BaseAddress = options.BaseUri;
    client.Timeout = options.RequestTimeout;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
