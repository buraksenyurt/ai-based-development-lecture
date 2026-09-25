using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages.Participants;

public class EditModel(ParticipantService participants) : PageModel
{
    [BindProperty]
    public ParticipantInput Input { get; set; } = new();

    public bool IsNew { get; private set; }

    public IActionResult OnGet(Guid? id)
    {
        IsNew = id is null;
        if (id is null)
            return Page();

        try
        {
            var participant = participants.Get(id.Value);
            Input = new ParticipantInput
            {
                FullName = participant.FullName,
                Email = participant.Email,
                University = participant.University,
                Department = participant.Department,
                Class = participant.Class,
                GithubUrl = participant.GithubUrl,
                Languages = ListInput.Join(participant.Languages),
                Databases = ListInput.Join(participant.Databases),
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
            var participant = new Participant(
                id ?? Guid.NewGuid(),
                Input.FullName!,
                Input.Email!,
                Input.University!,
                Input.Department!,
                Input.Class,
                Input.GithubUrl,
                ListInput.Split(Input.Languages),
                ListInput.Split(Input.Databases));

            participants.Save(participant);
            TempData["Success"] = $"{participant.FullName} kaydedildi.";
            return RedirectToPage("Index");
        }
        catch (DomainRuleException ex)
        {
            ModelState.AddRuleError(ex);
            return Page();
        }
    }

    public class ParticipantInput
    {
        [Required(ErrorMessage = "İsim zorunludur."), Display(Name = "Ad Soyad")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Email zorunludur."), EmailAddress(ErrorMessage = "Geçerli bir email girin.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Okul zorunludur."), Display(Name = "Okul")]
        public string? University { get; set; }

        [Required(ErrorMessage = "Bölüm zorunludur."), Display(Name = "Bölüm / Branş")]
        public string? Department { get; set; }

        [Range(1, 10, ErrorMessage = "Sınıf 1 ile 10 arasında olmalıdır."), Display(Name = "Sınıf")]
        public int Class { get; set; } = 1;

        [Url(ErrorMessage = "Geçerli bir adres girin."), Display(Name = "GitHub hesabı")]
        public string? GithubUrl { get; set; }

        [Display(Name = "Programlama dilleri")]
        public string? Languages { get; set; }

        [Display(Name = "Veritabanları")]
        public string? Databases { get; set; }
    }
}
