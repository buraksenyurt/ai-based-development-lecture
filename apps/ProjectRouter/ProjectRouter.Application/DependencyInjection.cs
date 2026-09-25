using Microsoft.Extensions.DependencyInjection;
using ProjectRouter.Application.Routing;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Action<RoutingOptions>? configure = null)
    {
        var options = new RoutingOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<IMatchScorer, PreferenceMatchScorer>();
        services.AddSingleton<IProjectRouter, GreedyProjectRouter>();
        services.AddScoped<ParticipantService>();
        services.AddScoped<ProjectIdeaService>();
        services.AddScoped<CompetitionService>();

        return services;
    }
}
