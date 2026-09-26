using Moq;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Routing;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Tests;

public class CompetitionServiceDetailsTests
{
    private readonly Mock<ICompetitionRepository> _competitions = new();
    private readonly Mock<IParticipantRepository> _participants = new();
    private readonly Mock<IProjectIdeaRepository> _projects = new();
    private readonly Mock<IProjectRouter> _router = new();
    private readonly IMatchScorer _scorer = new PreferenceMatchScorer(new RoutingOptions());

    private readonly ProjectIdea _csharpProject = TestData.Project(["C#"], ["SQLite"], 1, 2, title: "A - CSharp");
    private readonly ProjectIdea _javaProject = TestData.Project(["Java"], ["MongoDB"], 1, 2, title: "B - Java");
    private readonly Participant _ada = TestData.Participant(["C#"], ["SQLite"], name: "Ada");
    private readonly Participant _bob = TestData.Participant(["Python"], ["Redis"], name: "Bob");
    private readonly Participant _cem = TestData.Participant(["C#"], ["SQLite"], name: "Cem");

    private CompetitionService CreateService() =>
        new(_competitions.Object, _participants.Object, _projects.Object, _router.Object, _scorer, TimeProvider.System);

    private Competition ArrangeCompetition(params Participant[] returnedParticipants)
    {
        var competition = new Competition(
            Guid.NewGuid(), "AI", "2026-27",
            [_csharpProject.Id, _javaProject.Id],
            [_ada.Id, _bob.Id, _cem.Id]);

        _competitions.Setup(r => r.GetById(competition.Id)).Returns(competition);
        _participants.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>()))
            .Returns(returnedParticipants.Length > 0 ? returnedParticipants : [_ada, _bob, _cem]);
        _projects.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([_javaProject, _csharpProject]);

        return competition;
    }

    private void Settle(Competition competition) =>
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_csharpProject.Id] = [_bob.Id, _ada.Id] },
            [_csharpProject, _javaProject],
            DateTime.UtcNow);

    [Fact]
    public void GetAll_ReturnsRepositoryResult()
    {
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [], []);
        _competitions.Setup(r => r.GetAll()).Returns([competition]);

        Assert.Same(competition, Assert.Single(CreateService().GetAll()));
    }

    [Fact]
    public void Get_Unknown_ThrowsNotFound()
    {
        Assert.Throws<NotFoundException>(() => CreateService().Get(Guid.NewGuid()));
    }

    [Fact]
    public void Delete_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        CreateService().Delete(id);

        _competitions.Verify(r => r.Delete(id), Times.Once);
    }

    [Fact]
    public void Save_NewCompetition_SavesWithoutSettlement()
    {
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [_csharpProject.Id], [_ada.Id]);

        CreateService().Save(competition);

        Assert.Null(competition.SettledAt);
        _competitions.Verify(r => r.Save(competition), Times.Once);
    }

    [Fact]
    public void Save_WhenProjectsChange_ClearsSettlement()
    {
        var existing = ArrangeCompetition();
        Settle(existing);

        var edited = new Competition(existing.Id, "AI", "2026-27", [_csharpProject.Id], [_ada.Id, _bob.Id, _cem.Id]);
        CreateService().Save(edited);

        Assert.Null(edited.SettledAt);
        Assert.Empty(edited.Settlement);
    }

    [Fact]
    public void ClearSettlement_ClearsAndSaves()
    {
        var competition = ArrangeCompetition();
        Settle(competition);

        CreateService().ClearSettlement(competition.Id);

        Assert.Null(competition.SettledAt);
        Assert.Empty(competition.Settlement);
        _competitions.Verify(r => r.Save(competition), Times.Once);
    }

    [Fact]
    public void ClearSettlement_Unknown_ThrowsNotFound()
    {
        Assert.Throws<NotFoundException>(() => CreateService().ClearSettlement(Guid.NewGuid()));
        _competitions.Verify(r => r.Save(It.IsAny<Competition>()), Times.Never);
    }

    [Fact]
    public void GetDetails_BeforeRouting_HasNoTeams()
    {
        var competition = ArrangeCompetition();

        var details = CreateService().GetDetails(competition.Id);

        Assert.False(details.IsSettled);
        Assert.Same(competition, details.Competition);
        Assert.Empty(details.Teams);
        Assert.Empty(details.UnassignedParticipants);
        Assert.Empty(details.ClosedProjects);
        Assert.Equal(["Ada", "Bob", "Cem"], details.Participants.Select(p => p.FullName));
        Assert.Equal(["A - CSharp", "B - Java"], details.Projects.Select(p => p.Title));
    }

    [Fact]
    public void GetDetails_AfterRouting_BuildsTeamsUnassignedAndClosedProjects()
    {
        var competition = ArrangeCompetition();
        Settle(competition);

        var details = CreateService().GetDetails(competition.Id);

        Assert.True(details.IsSettled);
        var team = Assert.Single(details.Teams);
        Assert.Same(_csharpProject, team.Project);

        // Members are ordered by score (best match first).
        Assert.Equal([_ada.Id, _bob.Id], team.Members.Select(m => m.Participant.Id));
        Assert.Equal(1.0, team.Members[0].Score, 3);
        Assert.Equal(0.0, team.Members[1].Score, 3);

        Assert.Equal([_cem.Id], details.UnassignedParticipants.Select(p => p.Id));
        Assert.Equal([_javaProject.Id], details.ClosedProjects.Select(p => p.Id));
    }

    [Fact]
    public void GetDetails_WhenAPlacedParticipantNoLongerExists_SkipsThem()
    {
        var competition = ArrangeCompetition(_ada, _cem);
        Settle(competition);

        var details = CreateService().GetDetails(competition.Id);

        var team = Assert.Single(details.Teams);
        var member = Assert.Single(team.Members);
        Assert.Same(_ada, member.Participant);
    }

    [Fact]
    public void Export_BeforeRouting_HasEmptySettlement()
    {
        var competition = ArrangeCompetition();

        var document = CreateService().Export(competition.Id);

        Assert.Equal(competition.Id.ToString(), document.Id);
        Assert.Equal("AI", document.Title);
        Assert.Empty(Assert.Single(document.Settlement));
    }

    [Fact]
    public void RunRouting_ReturnsRouterResult()
    {
        var competition = ArrangeCompetition();
        var result = new RoutingResult
        {
            Settlement = new Dictionary<Guid, IReadOnlyList<Guid>> { [_csharpProject.Id] = [_ada.Id] },
            Placements = [new Placement(_ada.Id, _csharpProject.Id, 1)],
            UnassignedParticipantIds = [_bob.Id, _cem.Id],
            ClosedProjectIds = [_javaProject.Id],
            Warnings = ["warning"],
        };
        _router.Setup(r => r.Route(It.IsAny<IReadOnlyCollection<Participant>>(), It.IsAny<IReadOnlyCollection<ProjectIdea>>()))
            .Returns(result);

        var returned = CreateService().RunRouting(competition.Id);

        Assert.Same(result, returned);
        Assert.Equal([_ada.Id], competition.Settlement[_csharpProject.Id]);
    }
}
