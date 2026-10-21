using LoopLab.Application.Abstractions;
using LoopLab.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LoopLab.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        return services;
    }
}
