using MessageBus.Interfaces;
using MessageBus.Interfaces.Messages;
using MessageBus.Nats.Messages;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;

namespace MessageBus.Nats;

public static class ConfigureDependencies
{
    public static IServiceCollection AddNatsMessageBus(
        this IServiceCollection services,
        Action<IConsumerRegistrator>? registerConsumers = null )
    {
        // Register consumers
        var consumerRegistrator = new ConsumerRegistrator( services );
        if ( registerConsumers is not null )
        {
            registerConsumers( consumerRegistrator );
        }
        services.AddTransient( _ => consumerRegistrator );
        
        // Register consumers handler
        services.AddTransient<IConsumersHandler, ConsumersHandler>();

        // Register publisher
        services.AddTransient<IMessagePublisher, MessagePublisher>();

        // Register NATS connection 
        services.AddTransient<IConnection>( _ => new ConnectionFactory().CreateConnection() );

        return services;
    }
}