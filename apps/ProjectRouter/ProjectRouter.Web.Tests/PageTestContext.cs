using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Routing;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Tests;

/// <summary>
/// Real application services on top of mocked repositories, plus helpers to give a PageModel
/// the HttpContext / TempData it needs outside of the ASP.NET Core pipeline.
/// </summary>
internal sealed class PageTestContext
{
    public Mock<IParticipantRepository> Participants { get; } = new();
    public Mock<IProjectIdeaRepository> Projects { get; } = new();
    public Mock<ICompetitionRepository> Competitions { get; } = new();

    /// <summary>The real routing algorithm by default; tests can swap in a mock.</summary>
    public IProjectRouter Router { get; set; }

    public PageTestContext()
    {
        Router = new GreedyProjectRouter(Scorer);
        Participants.Setup(r => r.GetAll()).Returns([]);
        Projects.Setup(r => r.GetAll()).Returns([]);
        Competitions.Setup(r => r.GetAll()).Returns([]);
    }

    public IMatchScorer Scorer { get; } = new PreferenceMatchScorer(new RoutingOptions());

    public ParticipantService ParticipantService => new(Participants.Object, Competitions.Object);
    public ProjectIdeaService ProjectIdeaService => new(Projects.Object, Competitions.Object);
    public CompetitionService CompetitionService =>
        new(Competitions.Object, Participants.Object, Projects.Object, Router, Scorer, TimeProvider.System);

    public static T Attach<T>(T page) where T : PageModel
    {
        var httpContext = new DefaultHttpContext();
        page.PageContext = new PageContext { HttpContext = httpContext };
        page.TempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        return page;
    }

    public static Participant Participant(string name, string language = "C#", string database = "SQLite") =>
        new(
            Guid.NewGuid(),
            new Identity(name, $"{name.ToLowerInvariant()}@example.com"),
            new School("ITU", "Computer Engineering", 3),
            "https://github.com/" + name.ToLowerInvariant(),
            [language],
            [database]);

    public static ProjectIdea Project(string title, int min = 1, int max = 3, string language = "C#", string database = "SQLite") =>
        new(
            Guid.NewGuid(),
            title,
            "Summary",
            new TechStack([language], ["web"], [database]),
            new TeamSize(min, max),
            ProjectSize.M)
        {
            Similar = ["Kahoot"],
            Tags = ["game"],
        };
}
