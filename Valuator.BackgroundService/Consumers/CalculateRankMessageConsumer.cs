using System.Globalization;
using Caches.Interfaces;
using MessageBus.Interfaces;
using Microsoft.Extensions.Logging;
using Valuator.Caches.CacheIds;

namespace Valuator.BackgroundService.Consumers;

public class CalculateRankMessageConsumer : IMessageConsumer
{
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    public CalculateRankMessageConsumer(
        ICacheService cacheService,
        ILogger<CalculateRankMessageConsumer> logger )
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public void Consume( string messageContent )
    {
        _logger.LogDebug( $"Consuming message. Consumer: {nameof( CalculateRankMessageConsumer )}, Message: {messageContent}" );

        var indexModelId = new IndexModelId( messageContent );

        string? text = _cacheService.Get( new TextId( indexModelId ).ToCacheKey() );
        if ( text is null )
        {
            return;
        }

        double rank = CalculateRank( text );
        _cacheService.Add(
            new RankId( indexModelId ).ToCacheKey(),
            rank.ToString( CultureInfo.InvariantCulture ) );
    }

    private double CalculateRank( string text )
    {
        int notAlphabetSymbolsNumber = text.Count( symbol => !Char.IsLetter( symbol ) );
        double rank = notAlphabetSymbolsNumber / ( double )text.Length;

        return rank;
    }
}