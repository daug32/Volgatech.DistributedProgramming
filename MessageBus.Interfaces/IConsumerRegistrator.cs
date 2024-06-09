using MessageBus.Interfaces.Messages;

namespace MessageBus.Interfaces;

public interface IConsumerRegistrator
{
    IConsumerRegistrator AddConsumerForMessage<TConsumer>( MessageId messageId )
        where TConsumer : IMessageConsumer;
}