using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Competitions;

public class IndexModel(CompetitionService competitions) : PageModel
{
    public IReadOnlyList<Competition> Competitions { get; private set; } = [];

    public void OnGet() => Competitions = competitions.GetAll();

    public IActionResult OnPostDelete(Guid id)
    {
        competitions.Delete(id);
        TempData["Success"] = "Turnuva silindi.";
        return RedirectToPage();
    }
}
