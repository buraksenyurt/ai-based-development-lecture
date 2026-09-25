using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Routing;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application.Tests;

public class ApplicationSetupTests
{
    private static ServiceProvider BuildProvider(Action<RoutingOptions>? configure = null)
    {
        var services = new ServiceCollection();
        services.AddApplication(configure);
        services.AddSingleton(Mock.Of<IParticipantRepository>());
        services.AddSingleton(Mock.Of<IProjectIdeaRepository>());
        services.AddSingleton(Mock.Of<ICompetitionRepository>());
        return services.BuildServiceProvider();
    }

    [Fact]
    public void AddApplication_RegistersRoutingAndServices()
    {
        using var provider = BuildProvider();
        using var scope = provider.CreateScope();

        Assert.IsType<PreferenceMatchScorer>(provider.GetRequiredService<IMatchScorer>());
        Assert.IsType<GreedyProjectRouter>(provider.GetRequiredService<IProjectRouter>());
        Assert.Same(TimeProvider.System, provider.GetRequiredService<TimeProvider>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<ParticipantService>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<ProjectIdeaService>());
        Assert.NotNull(scope.ServiceProvider.GetRequiredService<CompetitionService>());
    }

    [Fact]
    public void AddApplication_WithoutConfigure_UsesDefaultWeights()
    {
        using var provider = BuildProvider();

        var options = provider.GetRequiredService<RoutingOptions>();

        Assert.Equal(0.6, options.LanguageWeight);
        Assert.Equal(0.4, options.DatabaseWeight);
    }

    [Fact]
    public void AddApplication_AppliesConfiguredWeights()
    {
        using var provider = BuildProvider(o =>
        {
            o.LanguageWeight = 0.9;
            o.DatabaseWeight = 0.1;
        });

        var options = provider.GetRequiredService<RoutingOptions>();

        Assert.Equal(0.9, options.LanguageWeight);
        Assert.Equal(0.1, options.DatabaseWeight);
    }

    [Fact]
    public void PreferenceMatchScorer_WithZeroWeights_ReturnsZero()
    {
        var scorer = new PreferenceMatchScorer(new RoutingOptions { LanguageWeight = 0, DatabaseWeight = 0 });
        var participant = TestData.Participant(["C#"], ["SQLite"]);
        var project = TestData.Project(["C#"], ["SQLite"], 1, 2);

        Assert.Equal(0, scorer.Score(participant, project));
    }

    [Fact]
    public void Exceptions_HaveReadableMessages()
    {
        var id = Guid.NewGuid();

        Assert.Equal($"Project '{id}' was not found.", new NotFoundException("Project", id).Message);
        Assert.Equal("in use", new InUseException("in use").Message);
    }
}
