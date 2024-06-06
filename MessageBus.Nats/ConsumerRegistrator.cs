using MessageBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBus.Nats;

public class ConsumerRegistrator : IConsumerRegistrator
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<MessageId, Type> _consumers = new();

    public ConsumerRegistrator( IServiceCollection services )
    {
        _services = services;
    }

    public IConsumerRegistrator AddConsumerForMessage<TConsumer>( MessageId messageId )
        where TConsumer : IMessageConsumer 
    {
        if ( _consumers.ContainsKey( messageId ) )
        {
            throw new InvalidOperationException( $"Consumer already exists for the message. MessageId: {messageId}" );
        }

        _consumers[messageId] = typeof( TConsumer );
        _services.AddScoped( _consumers[messageId] );
        
        return this;
    }

    internal Dictionary<MessageId, Type> GetConsumers() => _consumers.ToDictionary( x => x.Key, x => x.Value );
}
