using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using ProjectRouter.Application.Routing;
using ProjectRouter.Domain;
using CompetitionDetailsModel = ProjectRouter.Web.Pages.Competitions.DetailsModel;
using CompetitionEditModel = ProjectRouter.Web.Pages.Competitions.EditModel;
using CompetitionIndexModel = ProjectRouter.Web.Pages.Competitions.IndexModel;

namespace ProjectRouter.Web.Tests.Pages;

public class CompetitionPagesTests
{
    private readonly PageTestContext _context = new();
    private readonly Participant _ali = PageTestContext.Participant("Ali");
    private readonly Participant _veli = PageTestContext.Participant("Veli");
    private readonly ProjectIdea _arena = PageTestContext.Project("Arena", min: 1, max: 3);
    private readonly ProjectIdea _large = PageTestContext.Project("Large", min: 5, max: 6);

    private CompetitionIndexModel IndexPage() => PageTestContext.Attach(new CompetitionIndexModel(_context.CompetitionService));
    private CompetitionDetailsModel DetailsPage() => PageTestContext.Attach(new CompetitionDetailsModel(_context.CompetitionService));
    private CompetitionEditModel EditPage() => PageTestContext.Attach(
        new CompetitionEditModel(_context.CompetitionService, _context.ParticipantService, _context.ProjectIdeaService));

    private Competition ArrangeCompetition()
    {
        var competition = new Competition(Guid.NewGuid(), "AI Cup", "2026-27", [_arena.Id, _large.Id], [_ali.Id, _veli.Id]);
        _context.Competitions.Setup(r => r.GetById(competition.Id)).Returns(competition);
        _context.Participants.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_ali, _veli]);
        _context.Projects.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_arena, _large]);
        return competition;
    }

    // ---- Index ----

    [Fact]
    public void Index_OnGet_ListsCompetitions()
    {
        var competition = new Competition(Guid.NewGuid(), "AI Cup", "2026-27", [], []);
        _context.Competitions.Setup(r => r.GetAll()).Returns([competition]);
        var page = IndexPage();

        page.OnGet();

        Assert.Same(competition, Assert.Single(page.Competitions));
    }

    [Fact]
    public void Index_OnPostDelete_DeletesAndRedirects()
    {
        var id = Guid.NewGuid();
        var page = IndexPage();

        var result = page.OnPostDelete(id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Success"]);
        _context.Competitions.Verify(r => r.Delete(id), Times.Once);
    }

    // ---- Edit ----

    [Fact]
    public void Edit_OnGet_New_SelectsEverything()
    {
        _context.Participants.Setup(r => r.GetAll()).Returns([_ali, _veli]);
        _context.Projects.Setup(r => r.GetAll()).Returns([_arena]);
        var page = EditPage();

        var result = page.OnGet(null);

        Assert.IsType<PageResult>(result);
        Assert.True(page.IsNew);
        Assert.Equal([_ali.Id, _veli.Id], page.Input.ParticipantIds);
        Assert.Equal([_arena.Id], page.Input.ProjectIds);
        Assert.Equal(2, page.AllParticipants.Count);
        Assert.Single(page.AllProjects);
    }

    [Fact]
    public void Edit_OnGet_Existing_FillsForm()
    {
        var competition = ArrangeCompetition();
        var page = EditPage();

        var result = page.OnGet(competition.Id);

        Assert.IsType<PageResult>(result);
        Assert.False(page.IsNew);
        Assert.Equal("AI Cup", page.Input.Title);
        Assert.Equal("2026-27", page.Input.Session);
        Assert.Equal([_ali.Id, _veli.Id], page.Input.ParticipantIds);
        Assert.Equal([_arena.Id, _large.Id], page.Input.ProjectIds);
    }

    [Fact]
    public void Edit_OnGet_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(EditPage().OnGet(Guid.NewGuid()));
    }

    [Fact]
    public void Edit_OnPost_InvalidModelState_ReloadsOptions()
    {
        _context.Participants.Setup(r => r.GetAll()).Returns([_ali]);
        var page = EditPage();
        page.ModelState.AddModelError("Input.Title", "required");

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        Assert.Single(page.AllParticipants);
        _context.Competitions.Verify(r => r.Save(It.IsAny<Competition>()), Times.Never);
    }

    [Fact]
    public void Edit_OnPost_New_SavesAndRedirectsToDetails()
    {
        var page = EditPage();
        page.Input = new CompetitionEditModel.CompetitionInput
        {
            Title = "AI Cup",
            Session = "2026-27",
            ParticipantIds = [_ali.Id],
            ProjectIds = [_arena.Id],
        };

        var result = page.OnPost(null);

        var redirect = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("Details", redirect.PageName);
        Assert.True(page.IsNew);
        Assert.NotNull(page.TempData["Success"]);
        _context.Competitions.Verify(r => r.Save(It.Is<Competition>(c => c.Title == "AI Cup")), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_Existing_KeepsIdAndSettlement()
    {
        var existing = ArrangeCompetition();
        existing.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id, _veli.Id] }, [_arena, _large], DateTime.UtcNow);
        var page = EditPage();
        page.Input = new CompetitionEditModel.CompetitionInput
        {
            Title = "Renamed",
            Session = "2026-27",
            ParticipantIds = [_ali.Id, _veli.Id],
            ProjectIds = [_arena.Id, _large.Id],
        };

        page.OnPost(existing.Id);

        Assert.False(page.IsNew);
        _context.Competitions.Verify(r => r.Save(It.Is<Competition>(c => c.Id == existing.Id && c.SettledAt != null)), Times.Once);
    }

    [Fact]
    public void Edit_OnPost_WhenDomainRuleFails_ShowsError()
    {
        var page = EditPage();
        page.Input = new CompetitionEditModel.CompetitionInput { Title = "   ", Session = "2026-27" };

        var result = page.OnPost(null);

        Assert.IsType<PageResult>(result);
        var error = Assert.Single(page.ModelState[string.Empty]!.Errors);
        Assert.Equal("Title cannot be empty.", error.ErrorMessage);
        _context.Competitions.Verify(r => r.Save(It.IsAny<Competition>()), Times.Never);
    }

    // ---- Details ----

    [Theory]
    [InlineData(1.0, "text-bg-success")]
    [InlineData(0.75, "text-bg-success")]
    [InlineData(0.5, "text-bg-warning")]
    [InlineData(0.0, "text-bg-danger")]
    public void Details_ScoreBadgeClass_DependsOnScore(double score, string expected)
    {
        Assert.Equal(expected, CompetitionDetailsModel.ScoreBadgeClass(score));
    }

    [Fact]
    public void Details_OnGet_LoadsDetailsAndCapacity()
    {
        var competition = ArrangeCompetition();
        var page = DetailsPage();

        var result = page.OnGet(competition.Id);

        Assert.IsType<PageResult>(result);
        Assert.Same(competition, page.Details.Competition);
        Assert.Equal(6, page.MinCapacity);
        Assert.Equal(9, page.MaxCapacity);
    }

    [Fact]
    public void Details_OnGet_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(DetailsPage().OnGet(Guid.NewGuid()));
    }

    [Fact]
    public void Details_OnPostRoute_RunsRoutingAndKeepsWarnings()
    {
        var competition = ArrangeCompetition();
        var page = DetailsPage();

        var result = page.OnPostRoute(competition.Id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Success"]);
        Assert.Contains("Large", page.RoutingWarnings); // the 5-6 project cannot be opened with 2 participants
        Assert.NotNull(competition.SettledAt);
        Assert.Equal(2, competition.Settlement[_arena.Id].Count);
    }

    [Fact]
    public void Details_OnPostRoute_WithoutWarnings_ClearsWarnings()
    {
        var competition = new Competition(Guid.NewGuid(), "AI Cup", "2026-27", [_arena.Id], [_ali.Id, _veli.Id]);
        _context.Competitions.Setup(r => r.GetById(competition.Id)).Returns(competition);
        _context.Participants.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_ali, _veli]);
        _context.Projects.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_arena]);
        var page = DetailsPage();
        page.RoutingWarnings = "old warning";

        page.OnPostRoute(competition.Id);

        Assert.Null(page.RoutingWarnings);
    }

    [Fact]
    public void Details_OnPostRoute_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(DetailsPage().OnPostRoute(Guid.NewGuid()));
    }

    [Fact]
    public void Details_OnPostRoute_WhenSettlementBreaksARule_ShowsError()
    {
        var competition = ArrangeCompetition();
        var router = new Mock<IProjectRouter>();
        router.Setup(r => r.Route(It.IsAny<IReadOnlyCollection<Participant>>(), It.IsAny<IReadOnlyCollection<ProjectIdea>>()))
            .Returns(new RoutingResult
            {
                // "Large" needs at least 5 members: Rule 02 is violated.
                Settlement = new Dictionary<Guid, IReadOnlyList<Guid>> { [_large.Id] = [_ali.Id] },
                Placements = [],
                UnassignedParticipantIds = [],
                ClosedProjectIds = [],
                Warnings = [],
            });
        _context.Router = router.Object;
        var page = DetailsPage();

        var result = page.OnPostRoute(competition.Id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.StartsWith($"[{Rules.Rule02}]", (string)page.TempData["Error"]!);
        _context.Competitions.Verify(r => r.Save(It.IsAny<Competition>()), Times.Never);
    }

    [Fact]
    public void Details_OnPostClear_ClearsSettlement()
    {
        var competition = ArrangeCompetition();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id] }, [_arena, _large], DateTime.UtcNow);
        var page = DetailsPage();

        var result = page.OnPostClear(competition.Id);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.NotNull(page.TempData["Success"]);
        Assert.Null(competition.SettledAt);
        _context.Competitions.Verify(r => r.Save(competition), Times.Once);
    }

    [Fact]
    public void Details_OnGetExport_ReturnsJsonFile()
    {
        var competition = ArrangeCompetition();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id] }, [_arena, _large], DateTime.UtcNow);
        var page = DetailsPage();

        var result = page.OnGetExport(competition.Id);

        var file = Assert.IsType<FileContentResult>(result);
        Assert.Equal("application/json", file.ContentType);
        Assert.Equal($"competition-{competition.Id}.json", file.FileDownloadName);

        using var json = JsonDocument.Parse(file.FileContents);
        Assert.Equal("AI Cup", json.RootElement.GetProperty("title").GetString());
        var settlement = json.RootElement.GetProperty("settlement")[0];
        Assert.Equal(_ali.Id.ToString(), settlement.GetProperty(_arena.Id.ToString())[0].GetString());
    }

    [Fact]
    public void Details_OnGetExport_Unknown_ReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(DetailsPage().OnGetExport(Guid.NewGuid()));
    }
}
