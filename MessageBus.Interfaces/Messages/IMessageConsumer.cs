namespace MessageBus.Interfaces.Messages;

public interface IMessageConsumer
{
    void Consume( string messageContent );
}