using MessageBus.Interfaces;

namespace MessageBus.Nats;

public interface IConsumerRegistrator
{
    ConsumerRegistrator AddConsumerForMessage<TConsumer>( MessageId messageId )
        where TConsumer : IMessageConsumer;
}

public class ConsumerRegistrator : IConsumerRegistrator
{
    private readonly Dictionary<MessageId, Type> _consumers = new();

    public ConsumerRegistrator AddConsumerForMessage<TConsumer>( MessageId messageId )
        where TConsumer : IMessageConsumer 
    {
        if ( _consumers.ContainsKey( messageId ) )
        {
            throw new InvalidOperationException( $"Consumer already exists for the message. MessageId: {messageId}" );
        }

        _consumers[messageId] = typeof( TConsumer );
        
        return this;
    }

    internal Dictionary<MessageId, Type> GetConsumers() => _consumers.ToDictionary( x => x.Key, x => x.Value );
}
