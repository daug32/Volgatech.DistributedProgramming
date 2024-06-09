using System.Text.Json;
using MessageBus.Interfaces.Messages;
using Microsoft.Extensions.Logging;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;

namespace Valuator.EventsLogger.Consumers;

public class SimilarityCalculatedConsumer( ILogger<SimilarityCalculatedConsumer> logger ) : IMessageConsumer
{
    public static MessageId MessageId => Messages.SimilarityCalculatedNotification;

    public void Consume( string messageContent )
    {
        logger.LogDebug( $"Consuming message. Consumer: {nameof( SimilarityCalculatedConsumer )}, Message: {messageContent}" );

        SimilarityCalculatedNotificationDto? similarityCalculatedMessageDto = JsonSerializer
            .Deserialize<SimilarityCalculatedNotificationDto>( messageContent );
        
        Console.WriteLine( 
            $"Событие: {nameof( SimilarityCalculatedNotificationDto)}\n"
            + $"\tId текста: \"{similarityCalculatedMessageDto?.TextId}\"\n"
            + $"\tSimilarity: \"{similarityCalculatedMessageDto?.Similarity}\"" );
    }
}