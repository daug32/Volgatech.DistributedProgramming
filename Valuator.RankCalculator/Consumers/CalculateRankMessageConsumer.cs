using System.Globalization;
using Caches.Extensions;
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
    private readonly IShardSearcher _shardSearcher;
    private readonly IMessagePublisher _messagePublisher;

    public static MessageId MessageId => Messages.CalculateRankRequest;

    public CalculateRankMessageConsumer(
        ILogger<CalculateRankMessageConsumer> logger,
        IMessagePublisher messagePublisher,
        IShardSearcher shardSearcher )
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
        _shardSearcher = shardSearcher;
    }

    public void Consume( string messageContent )
    {
        _logger.LogDebug( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var indexModelId = new IndexModelId( messageContent );
        var textId = new TextId( indexModelId );
        var shardKey = new ShardKey( textId );
        
        ICacheService? cacheService = _shardSearcher.Find( shardKey );
        if ( cacheService is null )
        {
            _logger.LogError( $"Couldn't find a cache shard for the shardKey. ShardKey: {shardKey}" );
            return;
        }
        
        _logger.LogInformation( $"LOOKUP: {textId}, {cacheService.Get( shardKey.ToCacheKey() )}" );

        string? text = cacheService.Get( textId.ToCacheKey() );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        cacheService.Add(
            new RankId( indexModelId ).ToCacheKey(),
            rank.ToString( CultureInfo.InvariantCulture ) );
        
        _messagePublisher.Publish(
            Messages.RankCalculatedNotification, 
            new RankCalculatedNotificationDto
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