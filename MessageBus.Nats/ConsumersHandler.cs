using System.Text;
using MessageBus.Interfaces;
using NATS.Client;

namespace MessageBus.Nats;

public class ConsumersHandler : IConsumersHandler, IDisposable
{
    private readonly IConnection _connection;
    private readonly ConsumerRegistrator _consumerRegistrator;
    private readonly IServiceProvider _serviceProvider;

    private List<ConsumerSubscription> _consumerSubscriptions;

    public ConsumersHandler( ConsumerRegistrator consumerRegistrator, IServiceProvider serviceProvider, IConnection connection )
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
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
        
        Dictionary<MessageId, Type> consumers = _consumerRegistrator.GetConsumers();

        foreach ( MessageId messageId in consumers.Keys )
        {
            Type consumerType = consumers[messageId];
            
            IMessageConsumer? consumer = _serviceProvider.GetService( consumerType ) as IMessageConsumer;
            if ( consumer is null )
            {
                throw new InvalidOperationException( $"Couldn't build a consumer. Type: {consumerType.FullName}" );
            }

            IAsyncSubscription subscription = _connection.SubscribeAsync(
                messageId.Subject,
                messageId.Queue,
                ( _, args ) =>
                {
                    string messageContent = Encoding.UTF8.GetString( args.Message.Data );
                    consumer.Consume( messageContent );
                } );
            
            consumerSubscriptions.Add( new ConsumerSubscription( consumer, subscription ) );
        }

        return consumerSubscriptions;
    }
}