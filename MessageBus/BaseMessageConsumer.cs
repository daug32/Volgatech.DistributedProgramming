using NATS.Client;

namespace MessageBus;

public abstract class BaseMessageConsumer : IDisposable
{
    // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
    private readonly IAsyncSubscription _subscription;
    
    public readonly MessageId MessageId;

    protected BaseMessageConsumer( MessageId messageId )
    {
        MessageId = messageId;

        _subscription = new ConnectionFactory()
            .CreateConnection()
            .SubscribeAsync( 
                messageId.Subject, 
                messageId.Queue,
                ( sender, args ) => Consume( args ) );
    }

    public void Start()
    {
        _subscription.Start();
    }

    public void Stop()
    {
        _subscription.Unsubscribe();
    }

    protected abstract void Consume( MsgHandlerEventArgs args );

    public void Dispose()
    {
        _subscription.Dispose();
    }
}