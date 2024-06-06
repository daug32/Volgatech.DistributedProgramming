using System.Globalization;
using Caches.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.CacheIds;

namespace Valuator.Pages;

public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly ICacheService _cacheService;

    public SummaryModel( ILogger<SummaryModel> logger, ICacheService cacheService )
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public double Rank { get; set; } = 0;
    public double Similarity { get; set; } = 0;

    public void OnGet( string id )
    {
        _logger.LogDebug( id );

        try
        {
            var indexedModelId = new IndexModelId( id );

            Rank = ParseDouble( _cacheService.Get( new RankId( indexedModelId ).ToCacheKey() ) );
            Similarity = ParseDouble( _cacheService.Get( new SimilarityId( indexedModelId ).ToCacheKey() ) );
        }
        catch ( Exception ex )
        {
            _logger.LogError( ex, null );
        }
    }

    private double ParseDouble( string? value )
    {
        return Double.TryParse( value, CultureInfo.InvariantCulture, out double result )
            ? result
            : 0;
    }
}