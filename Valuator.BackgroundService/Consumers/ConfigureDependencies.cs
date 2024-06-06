using Microsoft.Extensions.DependencyInjection;

namespace Valuator.BackgroundService.Consumers;

public static class ConfigureDependencies
{
    public static IServiceCollection AddConsumers( this IServiceCollection services )
    {
        services.AddScoped<CalculateRankMessageConsumer>();
        return services;
    }
}