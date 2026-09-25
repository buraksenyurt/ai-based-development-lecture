using ProjectRouter.Application.Routing;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Tests;

public class GreedyProjectRouterTests
{
    private readonly GreedyProjectRouter _router = new(new PreferenceMatchScorer(new RoutingOptions()));

    private static void AssertRulesHold(RoutingResult result, IEnumerable<ProjectIdea> projects)
    {
        var lookup = projects.ToDictionary(p => p.Id);

        // Rule 02: every opened project is within its team size range.
        foreach (var (projectId, members) in result.Settlement)
            Assert.True(lookup[projectId].Team.Allows(members.Count), $"Team size {members.Count} is outside {lookup[projectId].Team}.");

        // Rule 03: nobody is placed twice.
        var placed = result.Settlement.Values.SelectMany(m => m).ToList();
        Assert.Equal(placed.Count, placed.Distinct().Count());
    }

    [Fact]
    public void Route_PlacesParticipantsIntoTheirPreferredProjects()
    {
        var csharpProject = TestData.Project(["C#"], ["SQL Server"], 2, 3);
        var pythonProject = TestData.Project(["Python"], ["MongoDB"], 2, 3);
        var csharpFans = Enumerable.Range(0, 2).Select(_ => TestData.Participant(["C#"], ["SQL Server"])).ToList();
        var pythonFans = Enumerable.Range(0, 2).Select(_ => TestData.Participant(["Python"], ["MongoDB"])).ToList();

        var result = _router.Route([.. csharpFans, .. pythonFans], [csharpProject, pythonProject]);

        Assert.Equal(csharpFans.Select(p => p.Id).Order(), result.Settlement[csharpProject.Id].Order());
        Assert.Equal(pythonFans.Select(p => p.Id).Order(), result.Settlement[pythonProject.Id].Order());
        Assert.Empty(result.UnassignedParticipantIds);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void Route_FillsMinimumTeamSizeEvenWhenPreferencesAreUnbalanced()
    {
        // Everybody wants C#, but the C# project only has room for 3.
        var csharpProject = TestData.Project(["C#"], ["SQLite"], 2, 3);
        var goProject = TestData.Project(["Go"], ["Redis"], 2, 3);
        var participants = Enumerable.Range(0, 5).Select(_ => TestData.Participant(["C#"], ["SQLite"])).ToList();

        var result = _router.Route(participants, [csharpProject, goProject]);

        AssertRulesHold(result, [csharpProject, goProject]);
        Assert.Equal(3, result.Settlement[csharpProject.Id].Count);
        Assert.Equal(2, result.Settlement[goProject.Id].Count);
        Assert.Empty(result.UnassignedParticipantIds);
        Assert.Contains(result.Warnings, w => w.Contains("none of their preferences"));
    }

    [Fact]
    public void Route_DoesNotOpenProjectsThatCannotReachTheirMinimum()
    {
        var small = TestData.Project(["C#"], ["SQLite"], 2, 4);
        var large = TestData.Project(["C#"], ["SQLite"], 5, 6);
        var participants = Enumerable.Range(0, 3).Select(_ => TestData.Participant(["C#"], ["SQLite"])).ToList();

        var result = _router.Route(participants, [small, large]);

        AssertRulesHold(result, [small, large]);
        Assert.Equal([large.Id], result.ClosedProjectIds);
        Assert.Equal(3, result.Settlement[small.Id].Count);
    }

    [Fact]
    public void Route_ReportsUnassignedParticipantsWhenCapacityIsExceeded()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 1, 2);
        var participants = Enumerable.Range(0, 4).Select(_ => TestData.Participant(["C#"], ["SQLite"])).ToList();

        var result = _router.Route(participants, [project]);

        AssertRulesHold(result, [project]);
        Assert.Equal(2, result.Settlement[project.Id].Count);
        Assert.Equal(2, result.UnassignedParticipantIds.Count);
        Assert.Contains(result.Warnings, w => w.Contains("could not be placed"));
    }

    [Fact]
    public void Route_DoesNotOpenUnwantedProjectsWhenCapacityIsEnough()
    {
        var wanted = TestData.Project(["C#"], ["SQLite"], 2, 4);
        var unwanted = TestData.Project(["COBOL"], ["DB2"], 1, 4);
        var participants = Enumerable.Range(0, 4).Select(_ => TestData.Participant(["C#"], ["SQLite"])).ToList();

        var result = _router.Route(participants, [wanted, unwanted]);

        Assert.Equal(4, result.Settlement[wanted.Id].Count);
        Assert.Contains(unwanted.Id, result.ClosedProjectIds);
    }

    [Fact]
    public void Route_IsDeterministic()
    {
        var projects = new[]
        {
            TestData.Project(["C#"], ["SQL Server"], 2, 3),
            TestData.Project(["Python"], ["MongoDB"], 2, 4),
            TestData.Project(["Java", "Kotlin"], ["PostgreSQL"], 2, 3),
        };
        var participants = new[]
        {
            TestData.Participant(["C#", "Python"], ["SQL Server"]),
            TestData.Participant(["Python"], ["NoSQL-*"]),
            TestData.Participant(["Kotlin", "C#"], ["PostgreSQL"]),
            TestData.Participant(["Java"], ["MySQL", "PostgreSQL"]),
            TestData.Participant(["Python", "Java"], ["MongoDB"]),
            TestData.Participant(["C#"], ["PostgreSQL"]),
            TestData.Participant(["Go"], ["Redis"]),
            TestData.Participant(["Python"], ["SQL Server"]),
        };

        var first = _router.Route(participants, projects);
        var second = _router.Route(participants.Reverse().ToArray(), projects.Reverse().ToArray());

        AssertRulesHold(first, projects);
        Assert.Equal(first.Placements.OrderBy(p => p.ParticipantId), second.Placements.OrderBy(p => p.ParticipantId));
        Assert.Empty(first.UnassignedParticipantIds);
    }

    [Fact]
    public void Route_WithNoParticipants_ClosesAllProjects()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 1, 2);

        var result = _router.Route([], [project]);

        Assert.Empty(result.Settlement);
        Assert.Equal([project.Id], result.ClosedProjectIds);
    }
}
