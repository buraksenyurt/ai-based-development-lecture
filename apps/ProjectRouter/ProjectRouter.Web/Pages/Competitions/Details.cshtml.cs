using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
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
