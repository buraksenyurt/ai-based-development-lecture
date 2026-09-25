using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class CompetitionSettlementTests
{
    private static ProjectIdea Project(int min = 1, int max = 3) =>
        new(Guid.NewGuid(), "Project", "Summary", new TechStack(["C#"], null, ["SQLite"]), new TeamSize(min, max), ProjectSize.S);

    [Fact]
    public void Constructor_RemovesDuplicateIdsAndTrimsText()
    {
        var projectId = Guid.NewGuid();
        var participantId = Guid.NewGuid();

        var competition = new Competition(Guid.NewGuid(), "  AI Cup ", " 2026-27 ", [projectId, projectId], [participantId, participantId]);

        Assert.Equal("AI Cup", competition.Title);
        Assert.Equal("2026-27", competition.Session);
        Assert.Single(competition.ProjectIds);
        Assert.Single(competition.ParticipantIds);
        Assert.Empty(competition.Settlement);
        Assert.Null(competition.SettledAt);
    }

    [Fact]
    public void Constructor_WithEmptyId_Throws()
    {
        var ex = Assert.Throws<DomainRuleException>(() => new Competition(Guid.Empty, "AI", "2026-27", [], []));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Theory]
    [InlineData("", "2026-27")]
    [InlineData("AI", " ")]
    public void Constructor_WithBlankTitleOrSession_Throws(string title, string session)
    {
        var ex = Assert.Throws<DomainRuleException>(() => new Competition(Guid.NewGuid(), title, session, [], []));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void AssignSettlement_WithNullSettlement_Throws()
    {
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [], []);

        Assert.Throws<ArgumentNullException>(() => competition.AssignSettlement(null!, [], DateTime.UtcNow));
    }

    [Fact]
    public void AssignSettlement_WithProjectOutsideCompetition_Throws()
    {
        var project = Project();
        var participantId = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [], [participantId]);

        var ex = Assert.Throws<DomainRuleException>(() => competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = [participantId] }, [project], DateTime.UtcNow));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void AssignSettlement_WithProjectMissingFromGivenProjects_Throws()
    {
        var project = Project();
        var participantId = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [participantId]);

        var ex = Assert.Throws<DomainRuleException>(() => competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [project.Id] = [participantId] }, [], DateTime.UtcNow));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void AssignSettlement_SkipsProjectsWithoutMembers()
    {
        var opened = Project();
        var empty = Project(min: 2, max: 4);
        var participantId = Guid.NewGuid();
        var settledAt = new DateTime(2026, 9, 25, 10, 0, 0, DateTimeKind.Utc);
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [opened.Id, empty.Id], [participantId]);

        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [opened.Id] = [participantId], [empty.Id] = [] },
            [opened, empty],
            settledAt);

        var members = Assert.Single(competition.Settlement).Value;
        Assert.Equal([participantId], members);
        Assert.False(competition.Settlement.ContainsKey(empty.Id));
        Assert.Equal(settledAt, competition.SettledAt);
    }

    [Fact]
    public void RestoreSettlement_ReplacesSettlementAndDate()
    {
        var projectId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var settledAt = DateTime.UtcNow;
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [projectId], [participantId]);

        competition.RestoreSettlement(new Dictionary<Guid, IReadOnlyList<Guid>> { [projectId] = [participantId] }, settledAt);

        Assert.Equal([participantId], competition.Settlement[projectId]);
        Assert.Equal(settledAt, competition.SettledAt);
    }
}
