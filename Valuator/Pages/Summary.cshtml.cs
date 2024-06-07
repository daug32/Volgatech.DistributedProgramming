using System.Globalization;
using Caches.Extensions;
using Caches.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.CacheIds;

namespace Valuator.Pages;

public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IShardSearcher _shardSearcher;

    public SummaryModel( ILogger<SummaryModel> logger, IShardSearcher shardSearcher )
    {
        _logger = logger;
        _shardSearcher = shardSearcher;
    }

    public double Rank { get; set; } = 0;
    public double Similarity { get; set; } = 0;

    public void OnGet( string id )
    {
        _logger.LogDebug( id );

        var indexedModelId = new IndexModelId( id );
        var textId = new TextId( indexedModelId );
        var shardKey = new ShardKey( textId );

        ICacheService? cacheService = _shardSearcher.Find( shardKey );
        if ( cacheService is null )
        {
            _logger.LogError( $"Couldn't find a cache shard for the shardKey. ShardKey: {shardKey}" );
            return;
        }
        
        _logger.LogInformation( $"LOOKUP: {textId}, {cacheService.Get( shardKey.ToCacheKey() )}" );

        Rank = ParseDouble( cacheService.Get( new RankId( indexedModelId ).ToCacheKey() ) );
        Similarity = ParseDouble( cacheService.Get( new SimilarityId( indexedModelId ).ToCacheKey() ) );
    }

    private double ParseDouble( string? value )
    {
        return Double.TryParse( value, CultureInfo.InvariantCulture, out double result )
            ? result
            : 0;
    }
}