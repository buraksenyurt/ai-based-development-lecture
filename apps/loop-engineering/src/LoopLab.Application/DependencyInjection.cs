using LoopLab.Application.Orders;
using Microsoft.Extensions.DependencyInjection;

namespace LoopLab.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<OrderDiscountCalculator>();
        services.AddSingleton<ShippingCostCalculator>();
        services.AddSingleton<OrderValidator>();
        services.AddScoped<OrderService>();
        return services;
    }
}
