using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Competitions;

public class EditModel(
    CompetitionService competitions,
    ParticipantService participants,
    ProjectIdeaService projects) : PageModel
{
    [BindProperty]
    public CompetitionInput Input { get; set; } = new();

    public bool IsNew { get; private set; }
    public IReadOnlyList<Participant> AllParticipants { get; private set; } = [];
    public IReadOnlyList<ProjectIdea> AllProjects { get; private set; } = [];

    public IActionResult OnGet(Guid? id)
    {
        IsNew = id is null;
        LoadOptions();

        if (id is null)
        {
            // A new competition starts with everything selected; the instructor unticks what is not needed.
            Input.ParticipantIds = AllParticipants.Select(p => p.Id).ToList();
            Input.ProjectIds = AllProjects.Select(p => p.Id).ToList();
            return Page();
        }

        try
        {
            var competition = competitions.Get(id.Value);
            Input = new CompetitionInput
            {
                Title = competition.Title,
                Session = competition.Session,
                ParticipantIds = competition.ParticipantIds.ToList(),
                ProjectIds = competition.ProjectIds.ToList(),
            };
            return Page();
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    public IActionResult OnPost(Guid? id)
    {
        IsNew = id is null;
        if (!ModelState.IsValid)
        {
            LoadOptions();
            return Page();
        }

        try
        {
            var competition = new Competition(
                Input.Title!,
                Input.Session!,
                Input.ProjectIds,
                Input.ParticipantIds);

            competitions.Save(competition);
            TempData["Success"] = $"{competition.Title} kaydedildi.";
            return RedirectToPage("Details", new { id = competition.Id });
        }
        catch (DomainRuleException ex)
        {
            ModelState.AddRuleError(ex);
            LoadOptions();
            return Page();
        }
    }

    private void LoadOptions()
    {
        AllParticipants = participants.GetAll();
        AllProjects = projects.GetAll();
    }

    public class CompetitionInput
    {
        [Required(ErrorMessage = "Başlık zorunludur."), Display(Name = "Turnuva başlığı")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Dönem zorunludur."), Display(Name = "Dönem")]
        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Dönem 2026-27 biçiminde olmalıdır.")]
        public string? Session { get; set; }

        public List<Guid> ParticipantIds { get; set; } = [];
        public List<Guid> ProjectIds { get; set; } = [];
    }
}
