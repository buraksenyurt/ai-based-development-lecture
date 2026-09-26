using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using ProjectRouter.Domain;
using ParticipantEditModel = ProjectRouter.Web.Pages.Participants.EditModel;
using ParticipantIndexModel = ProjectRouter.Web.Pages.Participants.IndexModel;

namespace ProjectRouter.Web.Tests.Pages;

public class ParticipantPagesTests
{
    private static readonly string[] ExpectedLanguages = ["C#", "Python"];
    private static readonly string[] ExpectedDatabases = ["SQLite", "NoSQL-*"];

    private readonly PageTestContext _context = new();

    private ParticipantIndexModel IndexPage() => PageTestContext.Attach(new ParticipantIndexModel(_context.ParticipantService));
    private ParticipantEditModel EditPage() => PageTestContext.Attach(new ParticipantEditModel(_context.ParticipantService));

    private static ParticipantEditModel.ParticipantInput ValidInput() => new()
    {
        FullName = "Ada Lovelace",
        Email = "ada@example.com",
        University = "ITU",
        Department = "Computer Engineering",
        Class = 4,
        GithubUrl = "https://github.com/ada",
        Languages = "C#, Python",
        Databases = "SQLite, NoSQL-*",
    };

    [Fact]
    public void Index_OnGet_ListsParticipants()
    {
        var ali = PageTestContext.Participant("Ali");
        _context.Participants.Setup(r => r.GetAll()).Returns([ali]);
        var page = IndexPage();

        page.OnGet();

        Assert.Same(ali, Assert.Single(page.Participants));
    }

    [Fact]
    public void Index_OnPostDelete_DeletesAndRedirects()
    {
        var id = Guid.NewGuid();
        var page = IndexPage();

        var result = page.OnPostDelete(id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Success"]);
        _context.Participants.Verify(r => r.Delete(id), Times.Once);
    }

    [Fact]
    public void Index_OnPostDelete_WhenInUse_ShowsError()
    {
        var id = Guid.NewGuid();
        _context.Competitions.Setup(r => r.IsParticipantReferenced(id)).Returns(true);
        var page = IndexPage();

        var result = page.OnPostDelete(id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Error"]);
        _context.Participants.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Edit_OnGet_New_ReturnsEmptyForm()
    {
        var page = EditPage();

        var result = page.OnGet(null);

        Assert.IsType<PageResult>(result);
        Assert.True(page.IsNew);
        Assert.Null(page.Input.FullName);
        Assert.Equal(1, page.Input.Class);
    }

    [Fact]
    public void Edit_OnGet_Existing_FillsForm()
    {
        var participant = new Participant(
            Guid.NewGuid(),
            new Identity("Ada Lovelace", "ada@example.com"),
            new School("ITU", "Computer Engineering", 4),
            "https://github.com/ada",
            ["C#", "Python"],
            ["SQLite", "Redis"]);
        _context.Participants.Setup(r => r.GetById(participant.Id)).Returns(participant);
        var page = EditPage();

        var result = page.OnGet(participant.Id);

        Assert.IsType<PageResult>(result);
        Assert.False(page.IsNew);
        Assert.Equal("Ada Lovelace", page.Input.FullName);
        Assert.Equal("ada@example.com", page.Input.Email);
        Assert.Equal("ITU", page.Input.University);
        Assert.Equal("Computer Engineering", page.Input.Department);
        Assert.Equal(4, page.Input.Class);
        Assert.Equal("https://github.com/ada", page.Input.GithubUrl);
        Assert.Equal("C#, Python", page.Input.Languages);
        Assert.Equal("SQLite, Redis", page.Input.Databases);
    }

    [Fact]
    public void Edit_OnGet_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(EditPage().OnGet(Guid.NewGuid()));
    }

    [Fact]
    public void Edit_OnPost_InvalidModelState_ReturnsPage()
    {
        var page = EditPage();
        page.ModelState.AddModelError("Input.FullName", "required");

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        _context.Participants.Verify(r => r.Save(It.IsAny<Participant>()), Times.Never);
    }

    [Fact]
    public void Edit_OnPost_New_SavesAndRedirects()
    {
        var page = EditPage();
        page.Input = ValidInput();

        var result = page.OnPost(null);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("Index", redirect.PageName);
        Assert.True(page.IsNew);
        Assert.NotNull(page.TempData["Success"]);
        _context.Participants.Verify(r => r.Save(It.Is<Participant>(p =>
            p.FullName == "Ada Lovelace" &&
            p.Languages.SequenceEqual(ExpectedLanguages) &&
            p.Databases.SequenceEqual(ExpectedDatabases))), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_Existing_KeepsId()
    {
        var id = Guid.NewGuid();
        var page = EditPage();
        page.Input = ValidInput();

        page.OnPost(id);

        Assert.False(page.IsNew);
        _context.Participants.Verify(r => r.Save(It.Is<Participant>(p => p.Id == id)), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_WhenDomainRuleFails_ShowsRuleError()
    {
        var page = EditPage();
        page.Input = ValidInput();
        page.Input.Languages = " , ";

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        var error = Assert.Single(page.ModelState[string.Empty]!.Errors);
        Assert.StartsWith($"[{Rules.Rule00}]", error.ErrorMessage);
        _context.Participants.Verify(r => r.Save(It.IsAny<Participant>()), Times.Never);
    }
}
