using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class EntityGuardTests
{
    private static readonly Identity Identity = new("Ada Lovelace", "ada@example.com");
    private static readonly School School = new("ITU", "Computer Engineering", 3);
    private static readonly TechStack Stack = new(["C#"], ["web"], ["SQLite"]);

    [Fact]
    public void Participant_WithoutIdentity_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Participant(Guid.NewGuid(), null!, School, null, ["C#"], ["SQLite"]));
    }

    [Fact]
    public void Participant_WithoutSchool_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new Participant(Guid.NewGuid(), Identity, null!, null, ["C#"], ["SQLite"]));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("   ")]
    public void Participant_WithBlankGithubUrl_StoresNull(string? githubUrl)
    {
        var participant = new Participant(Guid.NewGuid(), Identity, School, githubUrl, ["C#"], ["SQLite"]);

        Assert.Null(participant.GithubUrl);
    }

    [Fact]
    public void Participant_ExposesIdentityAndSchoolValues()
    {
        var participant = new Participant(Guid.NewGuid(), Identity, School, " https://github.com/ada ", ["C#"], ["SQLite"]);

        Assert.Equal("https://github.com/ada", participant.GithubUrl);
        Assert.Equal("Ada Lovelace", participant.FullName);
        Assert.Equal("ada@example.com", participant.Email);
        Assert.Equal("ITU", participant.University);
        Assert.Equal("Computer Engineering", participant.Department);
        Assert.Equal(3, participant.Class);
        Assert.Same(Identity, participant.Identity);
        Assert.Same(School, participant.School);
    }

    [Fact]
    public void Participant_WithDuplicateDatabases_ViolatesRule01()
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new Participant(Guid.NewGuid(), Identity, School, null, ["C#"], ["Redis", "redis"]));

        Assert.Equal(Rules.Rule01, ex.RuleCode);
    }

    [Fact]
    public void ProjectIdea_WithoutTechStack_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", null!, new TeamSize(1, 2), ProjectSize.S));
    }

    [Fact]
    public void ProjectIdea_WithDefaultTeamSize_ViolatesRule02()
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", Stack, default, ProjectSize.S));

        Assert.Equal(Rules.Rule02, ex.RuleCode);
    }

    [Fact]
    public void ProjectIdea_WithEmptyId_Throws()
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new ProjectIdea(Guid.Empty, "Quiz", "Summary", Stack, new TeamSize(1, 2), ProjectSize.S));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Theory]
    [InlineData("", "Summary")]
    [InlineData("Quiz", " ")]
    public void ProjectIdea_WithBlankTitleOrSummary_Throws(string title, string summary)
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new ProjectIdea(Guid.NewGuid(), title, summary, Stack, new TeamSize(1, 2), ProjectSize.S));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void ProjectIdea_SimilarAndTags_DefaultToEmpty()
    {
        var project = new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", Stack, new TeamSize(1, 2), ProjectSize.XL);

        Assert.Empty(project.Similar);
        Assert.Empty(project.Tags);
        Assert.Equal(ProjectSize.XL, project.Size);
        Assert.Same(Stack, project.TechStack);
    }

    [Fact]
    public void ProjectIdea_SimilarAndTags_AreTrimmedAndOrdered()
    {
        var project = new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", Stack, new TeamSize(1, 2), ProjectSize.S)
        {
            Similar = [" Kahoot ", "", "Quizizz"],
            Tags = ["education", " game "],
        };

        Assert.Equal(["Kahoot", "Quizizz"], project.Similar);
        Assert.Equal(["education", "game"], project.Tags);
    }

    [Fact]
    public void ProjectIdea_WithDuplicateTag_Throws()
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", Stack, new TeamSize(1, 2), ProjectSize.S)
            {
                Tags = ["game", "Game"],
            });

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void ProjectIdea_WithDuplicateSimilarProduct_Throws()
    {
        var ex = Assert.Throws<DomainRuleException>(() =>
            new ProjectIdea(Guid.NewGuid(), "Quiz", "Summary", Stack, new TeamSize(1, 2), ProjectSize.S)
            {
                Similar = ["Kahoot", "kahoot"],
            });

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }
}
