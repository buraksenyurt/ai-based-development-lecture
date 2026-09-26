using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectRouter.Domain;
using ProjectRouter.Web.Pages;

namespace ProjectRouter.Web.Tests.Pages;

public class HomeAndSharedPageTests
{
    [Fact]
    public void Index_OnGet_CountsEntities()
    {
        var context = new PageTestContext();
        context.Participants.Setup(r => r.GetAll()).Returns([PageTestContext.Participant("Ali"), PageTestContext.Participant("Veli")]);
        context.Projects.Setup(r => r.GetAll()).Returns([PageTestContext.Project("Arena")]);
        context.Competitions.Setup(r => r.GetAll()).Returns([new Competition(Guid.NewGuid(), "AI", "2026-27", [], [])]);
        var page = new IndexModel(context.ParticipantService, context.ProjectIdeaService, context.CompetitionService);

        page.OnGet();

        Assert.Equal(2, page.ParticipantCount);
        Assert.Equal(1, page.ProjectCount);
        Assert.Equal(1, page.CompetitionCount);
    }

    [Fact]
    public void Error_OnGet_SetsRequestId()
    {
        var page = PageTestContext.Attach(new ErrorModel());
        page.HttpContext.TraceIdentifier = "trace-123";

        Assert.False(page.ShowRequestId);

        page.OnGet();

        Assert.False(string.IsNullOrEmpty(page.RequestId));
        Assert.True(page.ShowRequestId);
    }

    [Theory]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    [InlineData(" C# , Go,, Rust ", new[] { "C#", "Go", "Rust" })]
    public void ListInput_Split_TrimsAndDropsEmptyEntries(string? value, string[] expected)
    {
        Assert.Equal(expected, ListInput.Split(value));
    }

    [Fact]
    public void ListInput_Join_UsesCommaSeparator()
    {
        Assert.Equal("C#, Go", ListInput.Join(["C#", "Go"]));
        Assert.Equal("", ListInput.Join([]));
    }

    [Fact]
    public void ListInput_AddRuleError_ForValidation_UsesPlainMessage()
    {
        var modelState = new ModelStateDictionary();

        modelState.AddRuleError(new DomainRuleException(DomainRuleException.Validation, "Title cannot be empty."));

        var error = Assert.Single(modelState[string.Empty]!.Errors);
        Assert.Equal("Title cannot be empty.", error.ErrorMessage);
    }

    [Fact]
    public void ListInput_AddRuleError_ForRule_PrefixesRuleCode()
    {
        var modelState = new ModelStateDictionary();

        modelState.AddRuleError(new DomainRuleException(Rules.Rule02, "Team is too small."));

        var error = Assert.Single(modelState[string.Empty]!.Errors);
        Assert.Equal("[Rule 02] Team is too small.", error.ErrorMessage);
    }
}
