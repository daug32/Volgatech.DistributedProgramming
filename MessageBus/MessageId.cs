using FluentAssertions;

namespace MessageBus;

public class MessageId
{
    public readonly string Subject;
    public readonly string Queue;

    public MessageId( string subject, string queue )
    {
        Subject = subject.ThrowIfNullOrEmpty();
        Queue = queue.ThrowIfNullOrEmpty();
    }

    public override string ToString() => $"Subject: {Subject}, Queue: {Queue}";
}