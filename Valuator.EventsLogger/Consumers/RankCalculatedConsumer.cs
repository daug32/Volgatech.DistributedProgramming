using System.Text.Json;
using MessageBus.Interfaces;
using Microsoft.Extensions.Logging;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;

namespace Valuator.EventsLogger.Consumers;

public class RankCalculatedConsumer : IMessageConsumer
{
    private readonly ILogger _logger;

    public static MessageId MessageId => Messages.RankCalculatedNotification;
    
    public RankCalculatedConsumer( ILogger<RankCalculatedConsumer> logger )
    {
        _logger = logger;
    }
    
    public void Consume( string messageContent )
    {
        _logger.LogDebug( $"Consuming message. Consumer: {nameof( SimilarityCalculatedConsumer )}, Message: {messageContent}" );

        SimilarityCalculatedNotificationDto? similarityCalculatedMessageDto = JsonSerializer.Deserialize<SimilarityCalculatedNotificationDto>( messageContent );
        
        Console.WriteLine( 
            $"Событие: {nameof( SimilarityCalculatedNotificationDto)}\n"
            + $"\tId текста: \"{similarityCalculatedMessageDto?.TextId}\"\n"
            + $"\tSimilarity: \"{similarityCalculatedMessageDto?.Similarity}\"" );
    }
}