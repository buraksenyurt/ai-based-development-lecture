using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Projects;

public class IndexModel(ProjectIdeaService projects) : PageModel
{
    public IReadOnlyList<ProjectIdea> Projects { get; private set; } = [];

    public void OnGet() => Projects = projects.GetAll();

    public IActionResult OnPostDelete(Guid id)
    {
        try
        {
            projects.Delete(id);
            TempData["Success"] = "Proje silindi.";
        }
        catch (InUseException)
        {
            TempData["Error"] = "Bu proje bir turnuvada yer alıyor. Önce turnuvadan çıkarın.";
        }

        return RedirectToPage();
    }
}
