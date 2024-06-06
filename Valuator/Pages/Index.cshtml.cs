using System.Globalization;
using Caches.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Extensions;
using Valuator.Models;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICacheService _cacheService;

    public IndexModel( ILogger<IndexModel> logger, ICacheService cacheService )
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost( string text )
    {
        _logger.LogDebug( text );

        var id = IndexModelId.New();
        double rank = CalculateRank( text );
        int similarity = CalculateSimilarity( text );

        _cacheService.Add( new TextId( id ).ToCacheKey(), text );
        _cacheService.Add( new SimilarityId( id ).ToCacheKey(), similarity.ToString( CultureInfo.InvariantCulture ) );
        _cacheService.Add( new RankId( id ).ToCacheKey(), rank.ToString( CultureInfo.InvariantCulture ) );

        return Redirect( $"summary?id={id}" );
    }

    private double CalculateRank( string text )
    {
        int notAlphabetSymbolsNumber = text.Count( symbol => !Char.IsLetter( symbol ) );
        double rank = notAlphabetSymbolsNumber / ( double )text.Length;

        return rank;
    }

    private int CalculateSimilarity( string text )
    {
        var keys = _cacheService.GetAllKeys();

        bool hasSameText = keys.Any( key => _cacheService
            .Get( key )
            !.Equals( text, StringComparison.InvariantCultureIgnoreCase ) );

        return hasSameText ? 1 : 0;
    }
}