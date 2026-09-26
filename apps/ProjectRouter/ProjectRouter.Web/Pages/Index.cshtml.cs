using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Web.Pages;

public class IndexModel(ParticipantService participants, ProjectIdeaService projects, CompetitionService competitions) : PageModel
{
    public int ParticipantCount { get; private set; }
    public int ProjectCount { get; private set; }
    public int CompetitionCount { get; private set; }

    public void OnGet()
    {
        ParticipantCount = participants.GetAll().Count;
        ProjectCount = projects.GetAll().Count;
        CompetitionCount = competitions.GetAll().Count;
    }
}
