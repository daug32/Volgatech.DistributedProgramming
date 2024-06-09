using FluentAssertions;

namespace MessageBus.Interfaces.Messages;

public class MessageId
{
    public readonly string Subject;
    
    /// <summary>
    /// If set, then messages will be consumed by one consumer with that name <br/>
    /// If not set, then messages will be consumed by all consumers that subscribed on that subject
    /// </summary>
    public readonly string? SubscriberName;

    public MessageId( string subject, string? subscriberName = null )
    {
        Subject = subject.ThrowIfNullOrEmpty();
        SubscriberName = subscriberName;
    }

    public override string ToString()
    {
        if ( SubscriberName is not null )
        {
            return $"Subject: {Subject}, Queue: {SubscriberName}";
        }
        
        return $"Subject: {Subject}";
    }
}