using MessageBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.Nats;

public static class ConfigureDependencies
{
    public static IServiceCollection AddNatsMessageBus(
        this IServiceCollection services,
        Action<IConsumerRegistrator>? registerConsumers = null )
    {
        var consumerRegistrator = new ConsumerRegistrator();
        if ( registerConsumers is not null )
        {
            registerConsumers( consumerRegistrator );
        }
        services.AddScoped( _ => consumerRegistrator );

        services.AddScoped<IConsumersHandler, ConsumersHandler>();
        services.AddScoped<IMessagePublisher, MessagePublisher>();

        return services;
    }
}