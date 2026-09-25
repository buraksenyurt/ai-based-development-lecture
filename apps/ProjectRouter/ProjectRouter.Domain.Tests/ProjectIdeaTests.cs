using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class ProjectIdeaTests
{
    private static ProjectIdea Create(string? summary = null, TechStack? techStack = null, TeamSize? team = null) =>
        new(
            "Kahoot Clone",
            summary ?? "Çevrimiçi bilgi yarışması platformu.",
            techStack ?? new TechStack(["python"], ["web", "mobile"], ["postgres", "mongodb"]),
            team ?? new TeamSize(2, 5),
            ProjectSize.L,
            ["Kahoot", "Mentimeter"],
            ["web", "game"]);

    [Fact]
    public void Constructor_WithValidData_CreatesProject()
    {
        var project = Create();

        Assert.Equal("Kahoot Clone", project.Title);
        Assert.Equal(2, project.Team.Min);
        Assert.Equal(5, project.Team.Max);
        Assert.Equal(["Kahoot", "Mentimeter"], project.Similar);
    }

    [Fact]
    public void TechStack_WithoutLanguage_ViolatesRule04()
    {
        var ex = Assert.Throws<DomainRuleException>(() => new TechStack([], ["web"], ["postgres"]));

        Assert.Equal(Rules.Rule04, ex.RuleCode);
    }

    [Fact]
    public void TechStack_WithoutDatabase_ViolatesRule04()
    {
        var ex = Assert.Throws<DomainRuleException>(() => new TechStack(["python"], ["web"], []));

        Assert.Equal(Rules.Rule04, ex.RuleCode);
    }

    [Fact]
    public void Constructor_WithSummaryLongerThan250Characters_Throws()
    {
        Assert.Throws<DomainRuleException>(() => Create(summary: new string('a', ProjectIdea.SummaryMaxLength + 1)));
    }

    [Fact]
    public void Constructor_WithSummaryOf250Characters_Succeeds()
    {
        var project = Create(summary: new string('a', ProjectIdea.SummaryMaxLength));

        Assert.Equal(ProjectIdea.SummaryMaxLength, project.Summary.Length);
    }

    [Theory]
    [InlineData(0, 3)]
    [InlineData(4, 2)]
    public void TeamSize_WithInvalidRange_ViolatesRule02(int min, int max)
    {
        var ex = Assert.Throws<DomainRuleException>(() => new TeamSize(min, max));

        Assert.Equal(Rules.Rule02, ex.RuleCode);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    public void TeamSize_Allows_ChecksInclusiveRange(int memberCount, bool expected)
    {
        Assert.Equal(expected, new TeamSize(2, 4).Allows(memberCount));
    }
}
