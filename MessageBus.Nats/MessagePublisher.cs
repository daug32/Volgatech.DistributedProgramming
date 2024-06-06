using System.Text;
using MessageBus.Interfaces;
using NATS.Client;

namespace MessageBus.Nats;

internal class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;

    public MessagePublisher( IConnection connection )
    {
        _connection = connection;
    }

    public void Publish( MessageId messageId, string content )
    {
        _connection.Publish( messageId.Subject, messageId.Queue, Encoding.UTF8.GetBytes( content ) );
    }
}