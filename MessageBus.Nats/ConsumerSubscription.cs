using MessageBus.Interfaces.Messages;
using NATS.Client;

namespace MessageBus.Nats;

internal record ConsumerSubscription( IMessageConsumer Consumer, IAsyncSubscription Subscription )
{
}