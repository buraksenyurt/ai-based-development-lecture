using McpServer.Services;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace McpServer.Tools;

[McpServerToolType]
public class BenchmarkTools
{
    [McpServerTool, Description("Sistemde koşan tüm projelerin (Zig, .NET, Rust vb.) genel log parser performans özetlerini getirir. Kullanıcı genel bir karşılaştırma istediğinde bu aracı kullan.")]
    public static async Task<string> GetAllBenchmarks(BenchmarkApiService apiService)
    {
        return await apiService.GetAllBenchmarksAsync();
    }

    [McpServerTool, Description("Belirli bir projeye (örn: LogParser_Zig veya LogParser_Rust) ait detaylı performans metriklerini (süre, bellek, versiyon) getirir.")]
    public static async Task<string> GetBenchmarkByName(
        BenchmarkApiService apiService,
        [Description("Performansı sorgulanacak projenin tam adı.")] string projectName)
    {
        return await apiService.GetBenchmarkByNameAsync(projectName);
    }

    [McpServerTool, Description("Elde edilen metrikleri analiz ettikten sonra sistemin dosya dizinine Markdown formatında kalıcı bir rapor kaydeder. Kullanıcı 'rapor oluştur', 'sonucu kaydet' dediğinde bunu kullan.")]
    public static async Task<string> CreateComplianceReport(
        BenchmarkApiService apiService,
        [Description("Oluşturulacak raporun kısa adı (boşluksuz, İngilizce karakter). Örn: zig_vs_rust_analizi")] string reportName,
        [Description("Raporun asıl içeriği. Markdown formatında başlıklar, listeler ve analiz yorumları içermelidir.")] string markdownContent)
    {
        return await apiService.CreateReportAsync(reportName, markdownContent);
    }
}
