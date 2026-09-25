using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;
using ProjectRouter.Application;
using ProjectRouter.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProjectRouter")
    ?? throw new InvalidOperationException("Connection string 'ProjectRouter' is not configured.");

builder.Services.AddRazorPages();
// Render Turkish characters as-is instead of HTML entities (e.g. "ı" rather than "&#x131;").
builder.Services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
builder.Services.AddApplication(options => builder.Configuration.GetSection("Routing").Bind(options));
builder.Services.AddData(connectionString);

var app = builder.Build();

app.Services.GetRequiredService<DatabaseInitializer>().Initialize();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
