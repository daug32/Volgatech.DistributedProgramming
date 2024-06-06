using MessageBus.Nats;
using Microsoft.Extensions.DependencyInjection;

namespace Valuator.BackgroundService.Consumers;

public static class ConfigureDependencies
{
    public static void AddConsumers( this IConsumerRegistrator registrator )
    {
        registrator.AddConsumerForMessage<CalculateRankMessageConsumer>( Messages.Messages.CalculateRankMessage );
    }
}