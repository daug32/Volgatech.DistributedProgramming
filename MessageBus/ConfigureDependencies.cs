using MessageBus.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus;

public static class ConfigureDependencies
{
    public static IServiceCollection AddNatsMessageBus(
        this IServiceCollection services )
    {
        services.AddScoped<IMessagePublisher, MessagePublisher>();
        return services;
    }
}