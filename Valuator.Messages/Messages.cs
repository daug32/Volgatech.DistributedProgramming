using MessageBus.Interfaces;

namespace Valuator.Messages;

public static class Messages
{
    public static MessageId CalculateRankMessage => new( "valuator.processing.rank", "rank_calculator" );
}