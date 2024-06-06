namespace MessageBus;

public interface IMessagePublisher
{
    void Publish( MessageId messageId, string content );
}