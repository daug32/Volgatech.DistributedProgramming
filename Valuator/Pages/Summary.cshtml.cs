using System.Globalization;
using Caches.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.CacheIds;
using Valuator.Caches.ShardSearching;

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

        ICacheService? cacheService = _shardSearcher.Find( textId.ToCacheKey() );
        if ( cacheService is null )
        {
            return;
        }

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