using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class ParticipantTests
{
    private static Participant Create(IEnumerable<string>? languages = null, IEnumerable<string>? databases = null, int @class = 4) =>
        new(
            Guid.NewGuid(),
            new Identity("Can Kulod Van Dam", "canklaud@marvel.corp.com"),
            new School("BatCave Technic", "Software Engineering", @class),
            "https://github.com/canklaudvandam",
            languages ?? ["C#", "Python"],
            databases ?? ["Sql Server", "NoSQL-*"]);

    [Fact]
    public void Constructor_WithValidData_KeepsPreferenceOrder()
    {
        var participant = Create(languages: [" C# ", "Python", "Go"]);

        Assert.Equal(["C#", "Python", "Go"], participant.Languages);
        Assert.Equal(["Sql Server", "NoSQL-*"], participant.Databases);
    }

    [Fact]
    public void Constructor_WithoutLanguages_ViolatesRule00()
    {
        var ex = Assert.Throws<DomainRuleException>(() => Create(languages: []));

        Assert.Equal(Rules.Rule00, ex.RuleCode);
    }

    [Fact]
    public void Constructor_WithOnlyBlankLanguages_ViolatesRule00()
    {
        var ex = Assert.Throws<DomainRuleException>(() => Create(languages: [" ", ""]));

        Assert.Equal(Rules.Rule00, ex.RuleCode);
    }

    [Fact]
    public void Constructor_WithDuplicateLanguages_ViolatesRule00()
    {
        var ex = Assert.Throws<DomainRuleException>(() => Create(languages: ["C#", "c#"]));

        Assert.Equal(Rules.Rule00, ex.RuleCode);
    }

    [Fact]
    public void Constructor_WithoutDatabases_ViolatesRule01()
    {
        var ex = Assert.Throws<DomainRuleException>(() => Create(databases: []));

        Assert.Equal(Rules.Rule01, ex.RuleCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidClass_Throws(int @class)
    {
        var ex = Assert.Throws<DomainRuleException>(() => Create(@class: @class));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void Constructor_WithInvalidEmail_Throws()
    {
        Assert.Throws<DomainRuleException>(() =>
            new Identity("Name", "not-an-email"));
    }

    [Fact]
    public void Constructor_WithEmptyId_Throws()
    {
        Assert.Throws<DomainRuleException>(() =>
            new Participant(Guid.Empty, new Identity("Name", "name@uni.edu"), new School("Uni", "Dept", 1), null, ["C#"], ["SQLite"]));
    }

    [Fact]
    public void Constructor_KeepsGivenId()
    {
        var id = Guid.NewGuid();

        var participant = new Participant(id, new Identity("Name", "name@uni.edu"), new School("Uni", "Dept", 1), null, ["C#"], ["SQLite"]);

        Assert.Equal(id, participant.Id);
    }
}
