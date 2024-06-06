namespace MessageBus.Interfaces;

public interface IMessagePublisher
{
    void Publish( MessageId messageId, string content );
}