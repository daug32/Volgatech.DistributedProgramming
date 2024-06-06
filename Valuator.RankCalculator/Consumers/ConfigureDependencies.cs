using MessageBus.Interfaces;

namespace Valuator.RankCalculator.Consumers;

public static class ConfigureDependencies
{
    public static void AddConsumers( this IConsumerRegistrator registrator )
    {
        registrator.AddConsumerForMessage<CalculateRankMessageConsumer>( Messages.Messages.CalculateRankMessage );
    }
}