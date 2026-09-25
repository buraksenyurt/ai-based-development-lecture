using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using ProjectRouter.Domain;
using ProjectEditModel = ProjectRouter.Web.Pages.Projects.EditModel;
using ProjectIndexModel = ProjectRouter.Web.Pages.Projects.IndexModel;

namespace ProjectRouter.Web.Tests.Pages;

public class ProjectPagesTests
{
    private static readonly string[] ExpectedLanguages = ["C#", "TypeScript"];
    private static readonly string[] ExpectedTags = ["education", "game"];

    private readonly PageTestContext _context = new();

    private ProjectIndexModel IndexPage() => PageTestContext.Attach(new ProjectIndexModel(_context.ProjectIdeaService));
    private ProjectEditModel EditPage() => PageTestContext.Attach(new ProjectEditModel(_context.ProjectIdeaService));

    private static ProjectEditModel.ProjectInput ValidInput() => new()
    {
        Title = "Quiz Arena",
        Summary = "Real-time quiz game",
        Languages = "C#, TypeScript",
        Platforms = "web",
        Databases = "PostgreSQL",
        TeamMin = 2,
        TeamMax = 4,
        Size = ProjectSize.L,
        Similar = "Kahoot",
        Tags = "education, game",
    };

    [Fact]
    public void Index_OnGet_ListsProjects()
    {
        var arena = PageTestContext.Project("Arena");
        _context.Projects.Setup(r => r.GetAll()).Returns([arena]);
        var page = IndexPage();

        page.OnGet();

        Assert.Same(arena, Assert.Single(page.Projects));
    }

    [Fact]
    public void Index_OnPostDelete_DeletesAndRedirects()
    {
        var id = Guid.NewGuid();
        var page = IndexPage();

        var result = page.OnPostDelete(id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Success"]);
        _context.Projects.Verify(r => r.Delete(id), Times.Once);
    }

    [Fact]
    public void Index_OnPostDelete_WhenInUse_ShowsError()
    {
        var id = Guid.NewGuid();
        _context.Competitions.Setup(r => r.IsProjectReferenced(id)).Returns(true);
        var page = IndexPage();

        var result = page.OnPostDelete(id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Error"]);
        _context.Projects.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Edit_OnGet_New_ReturnsDefaultForm()
    {
        var page = EditPage();

        var result = page.OnGet(null);

        Assert.IsType<PageResult>(result);
        Assert.True(page.IsNew);
        Assert.Equal(2, page.Input.TeamMin);
        Assert.Equal(4, page.Input.TeamMax);
        Assert.Equal(ProjectSize.M, page.Input.Size);
    }

    [Fact]
    public void Edit_OnGet_Existing_FillsForm()
    {
        var project = PageTestContext.Project("Arena", min: 2, max: 5);
        _context.Projects.Setup(r => r.GetById(project.Id)).Returns(project);
        var page = EditPage();

        var result = page.OnGet(project.Id);

        Assert.IsType<PageResult>(result);
        Assert.False(page.IsNew);
        Assert.Equal("Arena", page.Input.Title);
        Assert.Equal("Summary", page.Input.Summary);
        Assert.Equal("C#", page.Input.Languages);
        Assert.Equal("web", page.Input.Platforms);
        Assert.Equal("SQLite", page.Input.Databases);
        Assert.Equal(2, page.Input.TeamMin);
        Assert.Equal(5, page.Input.TeamMax);
        Assert.Equal(ProjectSize.M, page.Input.Size);
        Assert.Equal("Kahoot", page.Input.Similar);
        Assert.Equal("game", page.Input.Tags);
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
        page.ModelState.AddModelError("Input.Title", "required");

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        _context.Projects.Verify(r => r.Save(It.IsAny<ProjectIdea>()), Times.Never);
    }

    [Fact]
    public void Edit_OnPost_New_SavesAndRedirects()
    {
        var page = EditPage();
        page.Input = ValidInput();

        var result = page.OnPost(null);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("Index", redirect.PageName);
        Assert.NotNull(page.TempData["Success"]);
        _context.Projects.Verify(r => r.Save(It.Is<ProjectIdea>(p =>
            p.Title == "Quiz Arena" &&
            p.Team == new TeamSize(2, 4) &&
            p.Size == ProjectSize.L &&
            p.TechStack.Languages.SequenceEqual(ExpectedLanguages) &&
            p.Tags.SequenceEqual(ExpectedTags))), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_Existing_KeepsId()
    {
        var id = Guid.NewGuid();
        var page = EditPage();
        page.Input = ValidInput();

        page.OnPost(id);

        Assert.False(page.IsNew);
        _context.Projects.Verify(r => r.Save(It.Is<ProjectIdea>(p => p.Id == id)), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_WithInvalidTeamSize_ShowsRuleError()
    {
        var page = EditPage();
        page.Input = ValidInput();
        page.Input.TeamMin = 5;
        page.Input.TeamMax = 2;

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        var error = Assert.Single(page.ModelState[string.Empty]!.Errors);
        Assert.StartsWith($"[{Rules.Rule02}]", error.ErrorMessage);
        _context.Projects.Verify(r => r.Save(It.IsAny<ProjectIdea>()), Times.Never);
    }

    [Fact]
    public void Edit_OnPost_WithoutDatabase_ShowsRule04Error()
    {
        var page = EditPage();
        page.Input = ValidInput();
        page.Input.Databases = null;

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        var error = Assert.Single(page.ModelState[string.Empty]!.Errors);
        Assert.StartsWith($"[{Rules.Rule04}]", error.ErrorMessage);
    }
}
