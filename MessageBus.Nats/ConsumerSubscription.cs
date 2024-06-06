using MessageBus.Interfaces;
using NATS.Client;

namespace MessageBus.Nats;

internal class ConsumerSubscription
{
    public readonly IMessageConsumer Consumer;
    public readonly IAsyncSubscription Subscription;

    public ConsumerSubscription( IMessageConsumer consumer, IAsyncSubscription subscription )
    {
        Consumer = consumer;
        Subscription = subscription;
    }
}