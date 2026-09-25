using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Projects;

public class EditModel(ProjectIdeaService projects) : PageModel
{
    [BindProperty]
    public ProjectInput Input { get; set; } = new();

    public bool IsNew { get; private set; }

    public IActionResult OnGet(Guid? id)
    {
        IsNew = id is null;
        if (id is null)
            return Page();

        try
        {
            var project = projects.Get(id.Value);
            Input = new ProjectInput
            {
                Title = project.Title,
                Summary = project.Summary,
                Languages = ListInput.Join(project.TechStack.Languages),
                Platforms = ListInput.Join(project.TechStack.Platforms),
                Databases = ListInput.Join(project.TechStack.Databases),
                TeamMin = project.Team.Min,
                TeamMax = project.Team.Max,
                Size = project.Size,
                Similar = ListInput.Join(project.Similar),
                Tags = ListInput.Join(project.Tags),
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
            return Page();

        try
        {
            var project = new ProjectIdea(
                Input.Title!,
                Input.Summary!,
                new TechStack(ListInput.Split(Input.Languages), ListInput.Split(Input.Platforms), ListInput.Split(Input.Databases)),
                new TeamSize(Input.TeamMin, Input.TeamMax),
                Input.Size,
                ListInput.Split(Input.Similar),
                ListInput.Split(Input.Tags));

            projects.Save(project);
            TempData["Success"] = $"{project.Title} kaydedildi.";
            return RedirectToPage("Index");
        }
        catch (DomainRuleException ex)
        {
            ModelState.AddRuleError(ex);
            return Page();
        }
    }

    public class ProjectInput
    {
        [Required(ErrorMessage = "Proje adı zorunludur."), Display(Name = "Kısa ad")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Tarif zorunludur."), Display(Name = "Kısa tarif")]
        [StringLength(ProjectIdea.SummaryMaxLength, ErrorMessage = "Tarif en fazla {1} karakter olabilir.")]
        public string? Summary { get; set; }

        [Display(Name = "Programlama dilleri")]
        public string? Languages { get; set; }

        [Display(Name = "Platformlar")]
        public string? Platforms { get; set; }

        [Display(Name = "Veritabanları")]
        public string? Databases { get; set; }

        [Range(1, 50, ErrorMessage = "En az kişi sayısı 1 ile 50 arasında olmalıdır."), Display(Name = "En az kişi")]
        public int TeamMin { get; set; } = 2;

        [Range(1, 50, ErrorMessage = "En fazla kişi sayısı 1 ile 50 arasında olmalıdır."), Display(Name = "En fazla kişi")]
        public int TeamMax { get; set; } = 4;

        [Display(Name = "Büyüklük")]
        public ProjectSize Size { get; set; } = ProjectSize.M;

        [Display(Name = "Benzerleri")]
        public string? Similar { get; set; }

        [Display(Name = "Etiketler")]
        public string? Tags { get; set; }
    }
}
