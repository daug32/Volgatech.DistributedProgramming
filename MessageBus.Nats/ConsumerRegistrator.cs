using MessageBus.Interfaces;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.Nats;

public class ConsumerRegistrator( IServiceCollection services ) : IConsumerRegistrator
{
    private readonly Dictionary<MessageId, Type> _consumers = new();

    public IConsumerRegistrator AddConsumerForMessage<TConsumer>( MessageId messageId )
        where TConsumer : IMessageConsumer 
    {
        if ( _consumers.ContainsKey( messageId ) )
        {
            throw new InvalidOperationException( $"Consumer already exists for the message. MessageId: {messageId}" );
        }

        _consumers[messageId] = typeof( TConsumer );
        services.AddScoped( _consumers[messageId] );
        
        return this;
    }

    internal Dictionary<MessageId, Type> GetConsumers() => _consumers.ToDictionary( x => x.Key, x => x.Value );
}
