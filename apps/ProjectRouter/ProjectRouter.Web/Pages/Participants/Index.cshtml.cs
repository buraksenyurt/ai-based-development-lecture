using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Participants;

public class IndexModel(ParticipantService participants) : PageModel
{
    public IReadOnlyList<Participant> Participants { get; private set; } = [];

    public void OnGet() => Participants = participants.GetAll();

    public IActionResult OnPostDelete(Guid id)
    {
        try
        {
            participants.Delete(id);
            TempData["Success"] = "Katılımcı silindi.";
        }
        catch (InUseException)
        {
            TempData["Error"] = "Bu katılımcı bir turnuvada yer alıyor. Önce turnuvadan çıkarın.";
        }

        return RedirectToPage();
    }
}
