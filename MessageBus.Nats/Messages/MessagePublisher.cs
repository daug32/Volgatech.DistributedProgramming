using System.Text;
using System.Text.Json;
using MessageBus.Interfaces.Messages;
using NATS.Client;

namespace MessageBus.Nats.Messages;

internal class MessagePublisher( IConnection connection ) : IMessagePublisher
{
    public void Publish<T>( MessageId messageId, T content )
    {
        Publish( messageId, JsonSerializer.Serialize( content ) );
    }

    public void Publish( MessageId messageId, string content )
    {
        if ( messageId.SubscriberName is not null )
        {
            connection.Publish( messageId.Subject, messageId.SubscriberName, Encoding.UTF8.GetBytes( content ) );
            return;
        }
        
        connection.Publish( messageId.Subject, Encoding.UTF8.GetBytes( content ) );
    }
}