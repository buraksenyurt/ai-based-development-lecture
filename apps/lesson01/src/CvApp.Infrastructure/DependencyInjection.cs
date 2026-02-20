using CvApp.Domain.Interfaces;
using CvApp.Infrastructure.Persistence.Repositories;
using CvApp.Infrastructure.Persistence.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CvApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(nameof(MongoDbSettings)));

        services.AddScoped<IResumeRepository, ResumeRepository>();

        return services;
    }
}
