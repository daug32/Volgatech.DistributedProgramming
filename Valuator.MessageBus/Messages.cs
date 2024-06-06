using MessageBus.Interfaces;

namespace Valuator.MessageBus;

public static class Messages
{
    public static MessageId CalculateRankRequest => new( "valuator.processing.rank", "rank_calculator" );
    
    public static MessageId RankCalculatedNotification => new( "valuator.notification.rank", "events_listener" );
    public static MessageId SimilarityCalculatedNotification => new( "valuator.notification.similarity", "events_listener" );
}
