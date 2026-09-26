using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Reports;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Competitions;

public class DetailsModel(CompetitionService competitions) : PageModel
{
    private static readonly JsonSerializerOptions ExportJsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public CompetitionDetails Details { get; private set; } = null!;

    [TempData]
    public string? RoutingWarnings { get; set; }

    public int MinCapacity => Details.Projects.Sum(p => p.Team.Min);
    public int MaxCapacity => Details.Projects.Sum(p => p.Team.Max);

    /// <summary>Badge color of a match score: green for a strong match, yellow for a partial one, red for none.</summary>
    public static string ScoreBadgeClass(double score) => MatchLevels.Classify(score) switch
    {
        MatchLevel.Strong => "text-bg-success",
        MatchLevel.Partial => "text-bg-warning",
        _ => "text-bg-danger",
    };

    public IActionResult OnGet(Guid id)
    {
        try
        {
            Details = competitions.GetDetails(id);
            return Page();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    public IActionResult OnPostRoute(Guid id)
    {
        try
        {
            var result = competitions.RunRouting(id);
            TempData["Success"] = $"Dağıtım tamamlandı: {result.Placements.Count} katılımcı {result.Settlement.Count} projeye yerleştirildi.";
            RoutingWarnings = result.Warnings.Count > 0 ? string.Join("\n", result.Warnings) : null;
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (DomainRuleException ex)
        {
            TempData["Error"] = $"[{ex.RuleCode}] {ex.Message}";
        }

        return RedirectToPage(new { id });
    }

    public IActionResult OnPostClear(Guid id)
    {
        competitions.ClearSettlement(id);
        TempData["Success"] = "Dağıtım temizlendi.";
        return RedirectToPage(new { id });
    }

    /// <summary>Settlement as CSV (one row per participant), for Excel or further processing.</summary>
    public IActionResult OnGetExportCsv(Guid id)
    {
        try
        {
            var csv = SettlementCsv.WriteUtf8WithBom(competitions.GetDetails(id));
            return File(csv, "text/csv; charset=utf-8", $"dagitim-{id}.csv");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Compact HTML summary that opens in the browser; it can be saved, attached to an e-mail
    /// or copied into an e-mail body as it only uses inline styles.
    /// </summary>
    public IActionResult OnGetExportHtml(Guid id)
    {
        try
        {
            return Content(SettlementHtml.Write(competitions.GetDetails(id)), "text/html; charset=utf-8");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    public IActionResult OnGetExport(Guid id)
    {
        try
        {
            var document = competitions.Export(id);
            var json = JsonSerializer.SerializeToUtf8Bytes(document, ExportJsonOptions);
            return File(json, "application/json", $"competition-{id}.json");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
