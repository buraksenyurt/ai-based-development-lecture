using Moq;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Routing;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Tests;

public class CompetitionServiceTests
{
    private readonly Mock<ICompetitionRepository> _competitions = new();
    private readonly Mock<IParticipantRepository> _participants = new();
    private readonly Mock<IProjectIdeaRepository> _projects = new();
    private readonly Mock<IProjectRouter> _router = new();
    private readonly IMatchScorer _scorer = new PreferenceMatchScorer(new RoutingOptions());

    private CompetitionService CreateService() =>
        new(_competitions.Object, _participants.Object, _projects.Object, _router.Object, _scorer, TimeProvider.System);

    private (Competition Competition, ProjectIdea Project, Participant[] Participants) Arrange()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 2, 3);
        var participants = new[] { TestData.Participant(["C#"], ["SQLite"]), TestData.Participant(["C#"], ["SQLite"]) };
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], participants.Select(p => p.Id));

        _competitions.Setup(r => r.GetById(competition.Id)).Returns(competition);
        _participants.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns(participants);
        _projects.Setup(r => r.GetByIds(It.IsAny<IEnumerable<Guid>>())).Returns([project]);

        return (competition, project, participants);
    }

    [Fact]
    public void RunRouting_SavesValidSettlement()
    {
        var (competition, project, participants) = Arrange();
        _router.Setup(r => r.Route(It.IsAny<IReadOnlyCollection<Participant>>(), It.IsAny<IReadOnlyCollection<ProjectIdea>>()))
            .Returns(Result(new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = participants.Select(p => p.Id).ToList() }));

        CreateService().RunRouting(competition.Id);

        Assert.NotNull(competition.SettledAt);
        _competitions.Verify(r => r.Save(competition), Times.Once);
    }

    [Fact]
    public void RunRouting_WhenRouterBreaksRule02_DoesNotSave()
    {
        var (competition, project, participants) = Arrange();
        _router.Setup(r => r.Route(It.IsAny<IReadOnlyCollection<Participant>>(), It.IsAny<IReadOnlyCollection<ProjectIdea>>()))
            .Returns(Result(new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = [participants[0].Id] }));

        var ex = Assert.Throws<DomainRuleException>(() => CreateService().RunRouting(competition.Id));

        Assert.Equal(Rules.Rule02, ex.RuleCode);
        _competitions.Verify(r => r.Save(It.IsAny<Competition>()), Times.Never);
    }

    [Fact]
    public void RunRouting_UnknownCompetition_ThrowsNotFound()
    {
        Assert.Throws<NotFoundException>(() => CreateService().RunRouting(Guid.NewGuid()));
    }

    [Fact]
    public void Save_WhenMembersChange_ClearsSettlement()
    {
        var (competition, project, participants) = Arrange();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = participants.Select(p => p.Id).ToList() }, [project], DateTime.UtcNow);

        var edited = new Competition(competition.Id, "AI", "2026-27", [project.Id], [participants[0].Id]);
        CreateService().Save(edited);

        Assert.Null(edited.SettledAt);
        _competitions.Verify(r => r.Save(edited), Times.Once);
    }

    [Fact]
    public void Save_WhenOnlyTitleChanges_KeepsSettlement()
    {
        var (competition, project, participants) = Arrange();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = participants.Select(p => p.Id).ToList() }, [project], DateTime.UtcNow);

        var edited = new Competition(competition.Id, "Renamed", "2026-27", [project.Id], participants.Select(p => p.Id));
        CreateService().Save(edited);

        Assert.NotNull(edited.SettledAt);
        Assert.Equal(2, edited.Settlement[project.Id].Count);
    }

    [Fact]
    public void Export_UsesDocumentFormat()
    {
        var (competition, project, participants) = Arrange();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = participants.Select(p => p.Id).ToList() }, [project], DateTime.UtcNow);

        var document = CreateService().Export(competition.Id);

        Assert.Equal("2026-27", document.Session);
        var settlement = Assert.Single(document.Settlement);
        Assert.Equal(2, settlement[project.Id.ToString()].Count);
    }

    private static RoutingResult Result(Dictionary<Guid, IReadOnlyList<Guid>> settlement) => new()
    {
        Settlement = settlement,
        Placements = [],
        UnassignedParticipantIds = [],
        ClosedProjectIds = [],
        Warnings = [],
    };
}
