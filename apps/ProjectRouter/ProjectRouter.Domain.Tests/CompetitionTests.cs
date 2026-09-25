using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class CompetitionTests
{
    private static readonly DateTime Now = new(2026, 9, 25, 10, 0, 0, DateTimeKind.Utc);

    private static ProjectIdea Project(int min, int max) =>
        new(Guid.NewGuid(), $"Project {min}-{max}", "Summary", new TechStack(["C#"], [], ["SQLite"]), new TeamSize(min, max), ProjectSize.M, [], []);

    private static Dictionary<Guid, IReadOnlyList<Guid>> Settlement(params (Guid Project, Guid[] Members)[] teams) =>
        teams.ToDictionary(t => t.Project, t => (IReadOnlyList<Guid>)t.Members);

    [Fact]
    public void AssignSettlement_WithValidTeams_StoresSettlement()
    {
        var project = Project(2, 3);
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [p1, p2]);

        competition.AssignSettlement(Settlement((project.Id, [p1, p2])), [project], Now);

        Assert.Equal([p1, p2], competition.Settlement[project.Id]);
        Assert.Equal(Now, competition.SettledAt);
    }

    [Fact]
    public void AssignSettlement_WithTooSmallTeam_ViolatesRule02()
    {
        var project = Project(3, 4);
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [p1, p2]);

        var ex = Assert.Throws<DomainRuleException>(() =>
            competition.AssignSettlement(Settlement((project.Id, [p1, p2])), [project], Now));

        Assert.Equal(Rules.Rule02, ex.RuleCode);
        Assert.Null(competition.SettledAt);
    }

    [Fact]
    public void AssignSettlement_WithTooLargeTeam_ViolatesRule02()
    {
        var project = Project(1, 1);
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [p1, p2]);

        var ex = Assert.Throws<DomainRuleException>(() =>
            competition.AssignSettlement(Settlement((project.Id, [p1, p2])), [project], Now));

        Assert.Equal(Rules.Rule02, ex.RuleCode);
    }

    [Fact]
    public void AssignSettlement_WithParticipantInTwoProjects_ViolatesRule03()
    {
        var first = Project(1, 2);
        var second = Project(1, 2);
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [first.Id, second.Id], [p1, p2]);

        var ex = Assert.Throws<DomainRuleException>(() =>
            competition.AssignSettlement(Settlement((first.Id, [p1]), (second.Id, [p1, p2])), [first, second], Now));

        Assert.Equal(Rules.Rule03, ex.RuleCode);
    }

    [Fact]
    public void AssignSettlement_WithUnknownParticipant_Throws()
    {
        var project = Project(1, 2);
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [Guid.NewGuid()]);

        Assert.Throws<DomainRuleException>(() =>
            competition.AssignSettlement(Settlement((project.Id, [Guid.NewGuid()])), [project], Now));
    }

    [Fact]
    public void ClearSettlement_RemovesSettlement()
    {
        var project = Project(1, 2);
        var p1 = Guid.NewGuid();
        var competition = new Competition(Guid.NewGuid(), "AI", "2026-27", [project.Id], [p1]);
        competition.AssignSettlement(Settlement((project.Id, [p1])), [project], Now);

        competition.ClearSettlement();

        Assert.Empty(competition.Settlement);
        Assert.Null(competition.SettledAt);
    }
}
