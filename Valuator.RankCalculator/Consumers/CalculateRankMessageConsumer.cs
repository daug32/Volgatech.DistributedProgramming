using System.Globalization;
using Caches.Interfaces;
using MessageBus.Interfaces;
using Microsoft.Extensions.Logging;
using Valuator.Caches.CacheIds;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;

namespace Valuator.RankCalculator.Consumers;

public class CalculateRankMessageConsumer : IMessageConsumer
{
    private readonly ILogger _logger;
    private readonly ICacheService _cacheService;
    private readonly IMessagePublisher _messagePublisher;

    public static MessageId MessageId => Messages.CalculateRankRequest;

    public CalculateRankMessageConsumer(
        ICacheService cacheService,
        ILogger<CalculateRankMessageConsumer> logger,
        IMessagePublisher messagePublisher )
    {
        _cacheService = cacheService;
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public void Consume( string messageContent )
    {
        _logger.LogDebug( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var indexModelId = new IndexModelId( messageContent );
        var textId = new TextId( indexModelId );

        string? text = _cacheService.Get( textId.ToCacheKey() );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        _cacheService.Add(
            new RankId( indexModelId ).ToCacheKey(),
            rank.ToString( CultureInfo.InvariantCulture ) );
        
        _messagePublisher.Publish( Messages.RankCalculatedNotification, new RankCalculatedNotificationDto
        {
             Rank = rank,
             TextId = textId.ToString()
        } );
    }

    private double CalculateRank( string text )
    {
        int notAlphabetSymbolsNumber = text.Count( symbol => !Char.IsLetter( symbol ) );
        double rank = notAlphabetSymbolsNumber / ( double )text.Length;

        return rank;
    }
}