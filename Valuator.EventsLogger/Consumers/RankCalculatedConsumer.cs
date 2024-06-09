using System.Text.Json;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;

namespace Valuator.EventsLogger.Consumers;

public class RankCalculatedConsumer( ILogger<RankCalculatedConsumer> logger ) : IMessageConsumer
{
    public static MessageId MessageId => Messages.RankCalculatedNotification;

    public void Consume( string messageContent )
    {
        logger.LogDebug( $"Consuming message. Consumer: {nameof( SimilarityCalculatedConsumer )}, Message: {messageContent}" );

        RankCalculatedNotificationDto? rankCalculatedMessageDto = JsonSerializer
            .Deserialize<RankCalculatedNotificationDto>( messageContent );
        
        Console.WriteLine( 
            $"Событие: {nameof( RankCalculatedNotificationDto)}\n"
            + $"\tId текста: \"{rankCalculatedMessageDto?.TextId}\"\n"
            + $"\tRank: \"{rankCalculatedMessageDto?.Rank}\"" );
    }
}