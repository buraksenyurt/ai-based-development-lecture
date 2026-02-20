using CvApp.Application.Interfaces;
using CvApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CvApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IResumeService, ResumeService>();
        return services;
    }
}
