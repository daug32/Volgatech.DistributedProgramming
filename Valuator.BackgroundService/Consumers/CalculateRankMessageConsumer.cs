using System.Globalization;
using System.Text;
using Caches.Interfaces;
using MessageBus;
using MessageBus.Integration;
using Microsoft.Extensions.Logging;
using NATS.Client;
using Valuator.Caches.CacheIds;

namespace Valuator.BackgroundService.Consumers;

public class CalculateRankMessageConsumer : BaseMessageConsumer
{
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    public CalculateRankMessageConsumer(
        ICacheService cacheService,
        ILogger<CalculateRankMessageConsumer> logger )
        : base( Messages.CalculateRankMessage )
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    protected override void Consume( MsgHandlerEventArgs args )
    {
        string message = Encoding.UTF8.GetString( args.Message.Data );
        _logger.LogDebug( $"Consuming message. MessageId: {MessageId}, Message: {message}" );

        var indexModelId = new IndexModelId( message );

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