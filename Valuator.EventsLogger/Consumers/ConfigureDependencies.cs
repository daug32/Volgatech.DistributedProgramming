using MessageBus.Interfaces;

namespace Valuator.EventsLogger.Consumers;

public static class ConfigureDependencies
{
    public static void AddConsumers( this IConsumerRegistrator registrator )
    {
        registrator
            .AddConsumerForMessage<SimilarityCalculatedConsumer>( SimilarityCalculatedConsumer.MessageId )
            .AddConsumerForMessage<RankCalculatedConsumer>( RankCalculatedConsumer.MessageId );
    }
}