using System.Text;
using MessageBus.Interfaces;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using NATS.Client;

namespace MessageBus.Nats;

public class ConsumersHandler : IConsumersHandler, IDisposable
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerRegistrator _consumerRegistrator;
    private readonly ILogger _logger;

    private List<ConsumerSubscription> _consumerSubscriptions;

    public ConsumersHandler(
        ConsumerRegistrator consumerRegistrator,
        IServiceProvider serviceProvider,
        IConnection connection,
        ILogger<ConsumersHandler> logger )
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _logger = logger;
        _consumerRegistrator = consumerRegistrator;
    }

    public void Start()
    {
        _consumerSubscriptions = BuildConsumerSubscriptions();

        foreach ( ConsumerSubscription consumerSubscription in _consumerSubscriptions )
        {
            consumerSubscription.Subscription.Start();
        }
    }

    public void Stop()
    {
        foreach ( ConsumerSubscription consumerSubscription in _consumerSubscriptions )
        {
            consumerSubscription.Subscription.Unsubscribe();
        }
    }

    public void Dispose()
    {
        foreach ( ConsumerSubscription consumerSubscription in _consumerSubscriptions )
        {
            consumerSubscription.Subscription.Dispose();
        }
    }

    private List<ConsumerSubscription> BuildConsumerSubscriptions()
    {
        var consumerSubscriptions = new List<ConsumerSubscription>();

        var consumers = _consumerRegistrator.GetConsumers();

        foreach ( MessageId messageId in consumers.Keys )
        {
            Type consumerType = consumers[messageId];

            var consumer = _serviceProvider.GetService( consumerType ) as IMessageConsumer;
            if ( consumer is null )
            {
                throw new InvalidOperationException( $"Couldn't build a consumer. Type: {consumerType.FullName}" );
            }

            EventHandler<MsgHandlerEventArgs> eventHandler = ( _, args ) =>
            {
                try
                {
                    consumer.Consume( Encoding.UTF8.GetString( args.Message.Data ) );
                }
                catch ( Exception ex )
                {
                    _logger.LogCritical( ex, null );
                }
            };

            IAsyncSubscription subscription = messageId.SubscriberName is not null
                ? _connection.SubscribeAsync( messageId.Subject, messageId.SubscriberName, eventHandler )
                : _connection.SubscribeAsync( messageId.Subject, eventHandler );

            consumerSubscriptions.Add( new ConsumerSubscription( consumer, subscription ) );
        }

        return consumerSubscriptions;
    }
}