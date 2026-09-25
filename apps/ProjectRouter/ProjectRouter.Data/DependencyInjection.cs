using Microsoft.Extensions.DependencyInjection;
using ProjectRouter.Application.Interfaces;

namespace ProjectRouter.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new SqliteConnectionFactory(connectionString));
        services.AddSingleton<DatabaseInitializer>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IProjectIdeaRepository, ProjectIdeaRepository>();
        services.AddScoped<ICompetitionRepository, CompetitionRepository>();

        return services;
    }
}
