using System.Text;
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
                ( sender, args ) =>
                {
                    string message = Encoding.UTF8.GetString( args.Message.Data );
                    Consume( message );
                } );
    }

    public void Start()
    {
        _subscription.Start();
    }

    public void Stop()
    {
        _subscription.Unsubscribe();
    }

    protected abstract void Consume( string messageContent );

    public void Dispose()
    {
        _subscription.Dispose();
    }
}