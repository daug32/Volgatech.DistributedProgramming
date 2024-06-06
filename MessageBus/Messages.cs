namespace MessageBus.Integration;

public static class Messages
{
    public static MessageId CalculateRankMessage => new( "valuator.processing.rank", "rank_calculator" );
}