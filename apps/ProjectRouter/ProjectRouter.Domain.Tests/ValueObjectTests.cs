using ProjectRouter.Domain;

namespace ProjectRouter.Domain.Tests;

public class ValueObjectTests
{
    [Fact]
    public void Identity_TrimsValues()
    {
        var identity = new Identity("  Ada Lovelace ", " ada@example.com ");

        Assert.Equal("Ada Lovelace", identity.FullName);
        Assert.Equal("ada@example.com", identity.Email);
    }

    [Theory]
    [InlineData("", "ada@example.com")]
    [InlineData("Ada", "")]
    public void Identity_WithBlankValue_Throws(string fullName, string email)
    {
        var ex = Assert.Throws<DomainRuleException>(() => new Identity(fullName, email));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void Identity_WithSameValues_AreEqual()
    {
        Assert.Equal(new Identity("Ada", "ada@example.com"), new Identity("Ada", "ada@example.com"));
    }

    [Fact]
    public void School_TrimsValues()
    {
        var school = new School(" ITU ", " Computer Engineering ", 4);

        Assert.Equal("ITU", school.University);
        Assert.Equal("Computer Engineering", school.Department);
        Assert.Equal(4, school.Class);
    }

    [Theory]
    [InlineData("", "Computer Engineering")]
    [InlineData("ITU", " ")]
    public void School_WithBlankValue_Throws(string university, string department)
    {
        var ex = Assert.Throws<DomainRuleException>(() => new School(university, department, 1));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void TeamSize_ToString_ShowsRange()
    {
        Assert.Equal("2-4", new TeamSize(2, 4).ToString());
    }

    [Fact]
    public void TeamSize_WithSameRange_AreEqual()
    {
        Assert.Equal(new TeamSize(1, 3), new TeamSize(1, 3));
        Assert.NotEqual(new TeamSize(1, 3), new TeamSize(1, 4));
    }

    [Fact]
    public void TechStack_WithoutPlatforms_HasEmptyPlatformList()
    {
        var stack = new TechStack(["C#"], null, ["SQLite"]);

        Assert.Empty(stack.Platforms);
    }

    [Fact]
    public void TechStack_DropsBlankEntriesAndKeepsOrder()
    {
        var stack = new TechStack(["Go", " ", null!, "Rust"], [" web ", "mobile"], ["Redis"]);

        Assert.Equal(["Go", "Rust"], stack.Languages);
        Assert.Equal(["web", "mobile"], stack.Platforms);
        Assert.Equal(["Redis"], stack.Databases);
    }

    [Fact]
    public void TechStack_WithDuplicatePlatform_Throws()
    {
        var ex = Assert.Throws<DomainRuleException>(() => new TechStack(["C#"], ["web", "WEB"], ["SQLite"]));

        Assert.Equal(DomainRuleException.Validation, ex.RuleCode);
    }

    [Fact]
    public void TechStack_WithDuplicateDatabase_ViolatesRule04()
    {
        var ex = Assert.Throws<DomainRuleException>(() => new TechStack(["C#"], null, ["SQLite", "sqlite"]));

        Assert.Equal(Rules.Rule04, ex.RuleCode);
    }

    [Fact]
    public void DomainRuleException_KeepsRuleCodeAndMessage()
    {
        var ex = new DomainRuleException(Rules.Rule03, "message");

        Assert.Equal(Rules.Rule03, ex.RuleCode);
        Assert.Equal("message", ex.Message);
    }
}
