using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ProjectRouter.Application.Services;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Tests.Integration;

/// <summary>
/// Renders every page through the real pipeline (Program.cs, DI, SQLite, Razor views).
/// One database is shared by the class; each test seeds its own data with unique ids.
/// </summary>
public class PageRenderingTests : IClassFixture<ProjectRouterWebFactory>
{
    private readonly ProjectRouterWebFactory _factory;
    private readonly HttpClient _client;

    public PageRenderingTests(ProjectRouterWebFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private (Competition Competition, ProjectIdea Project, Participant Participant) Seed(bool route)
    {
        using var scope = _factory.Services.CreateScope();
        var participants = scope.ServiceProvider.GetRequiredService<ParticipantService>();
        var projects = scope.ServiceProvider.GetRequiredService<ProjectIdeaService>();
        var competitions = scope.ServiceProvider.GetRequiredService<CompetitionService>();

        var suffix = Guid.NewGuid().ToString("N")[..6];
        var participant = PageTestContext.Participant($"Ada{suffix}");
        var other = PageTestContext.Participant($"Bob{suffix}", language: "Python", database: "Redis");
        var project = PageTestContext.Project($"Arena {suffix}", min: 1, max: 2);
        var closed = PageTestContext.Project($"Large {suffix}", min: 5, max: 6);

        participants.Save(participant);
        participants.Save(other);
        projects.Save(project);
        projects.Save(closed);

        var competition = new Competition(
            Guid.NewGuid(), $"Cup {suffix}", "2026-27", [project.Id, closed.Id], [participant.Id, other.Id]);
        competitions.Save(competition);

        if (route)
            competitions.RunRouting(competition.Id);

        return (competition, project, participant);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Participants")]
    [InlineData("/Participants/Edit")]
    [InlineData("/Projects")]
    [InlineData("/Projects/Edit")]
    [InlineData("/Competitions")]
    [InlineData("/Competitions/Edit")]
    [InlineData("/Error")]
    public async Task Page_RendersSuccessfully(string url)
    {
        Seed(route: false);

        var response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.StartsWith("text/html", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task EditPages_RenderExistingEntities()
    {
        var (competition, project, participant) = Seed(route: false);

        var participantHtml = await _client.GetStringAsync($"/Participants/Edit/{participant.Id}");
        var projectHtml = await _client.GetStringAsync($"/Projects/Edit/{project.Id}");
        var competitionHtml = await _client.GetStringAsync($"/Competitions/Edit/{competition.Id}");

        Assert.Contains(participant.Email, participantHtml);
        Assert.Contains(project.Title, projectHtml);
        Assert.Contains(competition.Title, competitionHtml);
    }

    [Fact]
    public async Task Details_BeforeRouting_RendersCompetition()
    {
        var (competition, _, _) = Seed(route: false);

        var html = await _client.GetStringAsync($"/Competitions/Details/{competition.Id}");

        Assert.Contains(competition.Title, html);
    }

    [Fact]
    public async Task Details_AfterRouting_RendersTeams()
    {
        var (competition, project, participant) = Seed(route: true);

        var html = await _client.GetStringAsync($"/Competitions/Details/{competition.Id}");

        Assert.Contains(project.Title, html);
        Assert.Contains(participant.FullName, html);
    }

    [Fact]
    public async Task Details_Export_ReturnsJson()
    {
        var (competition, _, _) = Seed(route: true);

        var response = await _client.GetAsync($"/Competitions/Details/{competition.Id}?handler=Export");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains(competition.Id.ToString(), await response.Content.ReadAsStringAsync());
    }

    [Theory]
    [InlineData("/Competitions/Details/{0}")]
    [InlineData("/Competitions/Edit/{0}")]
    [InlineData("/Participants/Edit/{0}")]
    [InlineData("/Projects/Edit/{0}")]
    public async Task UnknownIds_ReturnNotFound(string urlFormat)
    {
        var response = await _client.GetAsync(string.Format(urlFormat, Guid.NewGuid()));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
