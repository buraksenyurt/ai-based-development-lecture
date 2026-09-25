using ProjectRouter.Application.Routing;

namespace ProjectRouter.Application.Tests;

public class PreferenceMatchScorerTests
{
    private readonly PreferenceMatchScorer _scorer = new(new RoutingOptions { LanguageWeight = 0.6, DatabaseWeight = 0.4 });

    [Fact]
    public void Score_FirstChoicesMatch_ReturnsOne()
    {
        var participant = TestData.Participant(["C#", "Python"], ["PostgreSQL"]);
        var project = TestData.Project(["C#"], ["PostgreSQL"], 1, 3);

        Assert.Equal(1.0, _scorer.Score(participant, project), 3);
    }

    [Fact]
    public void Score_SecondChoiceLanguage_GetsHalfOfLanguageWeight()
    {
        var participant = TestData.Participant(["C#", "Python"], ["PostgreSQL"]);
        var project = TestData.Project(["Python"], ["PostgreSQL"], 1, 3);

        Assert.Equal(0.6 * 0.5 + 0.4, _scorer.Score(participant, project), 3);
    }

    [Fact]
    public void Score_NothingMatches_ReturnsZero()
    {
        var participant = TestData.Participant(["Rust"], ["Redis"]);
        var project = TestData.Project(["Java"], ["Oracle"], 1, 3);

        Assert.Equal(0, _scorer.Score(participant, project));
    }

    [Fact]
    public void Score_IgnoresCaseWhitespaceAndAliases()
    {
        var participant = TestData.Participant(["csharp"], ["Sql Server"]);
        var project = TestData.Project(["C#"], ["sqlserver"], 1, 3);

        Assert.Equal(1.0, _scorer.Score(participant, project), 3);
    }

    [Fact]
    public void Score_PostgresAliasMatchesPostgreSql()
    {
        var participant = TestData.Participant(["python"], ["postgres"]);
        var project = TestData.Project(["Python"], ["PostgreSQL"], 1, 3);

        Assert.Equal(1.0, _scorer.Score(participant, project), 3);
    }

    [Fact]
    public void Score_WildcardDatabaseMatchesCategory()
    {
        var participant = TestData.Participant(["python"], ["Sql Server", "NoSQL-*"]);
        var project = TestData.Project(["python"], ["mongodb"], 1, 3);

        // Language: 1st choice (1.0). Database: "NoSQL-*" is the 2nd choice (0.5).
        Assert.Equal(0.6 + 0.4 * 0.5, _scorer.Score(participant, project), 3);
    }

    [Fact]
    public void Score_WildcardDoesNotMatchOtherCategory()
    {
        var participant = TestData.Participant(["python"], ["NoSQL-*"]);
        var project = TestData.Project(["python"], ["PostgreSQL"], 1, 3);

        Assert.Equal(0.6, _scorer.Score(participant, project), 3);
    }
}
