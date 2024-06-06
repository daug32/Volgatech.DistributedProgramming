namespace MessageBus.Interfaces;

public interface IMessageConsumer
{
    void Consume( string messageContent );
}