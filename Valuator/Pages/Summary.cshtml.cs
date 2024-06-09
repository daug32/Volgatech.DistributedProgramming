using System.Globalization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.ValueObjects;
using Valuator.Repositories.Interfaces;

namespace Valuator.Pages;

public class SummaryModel : PageModel
{
    private readonly ILogger<SummaryModel> _logger;
    private readonly IRankRepository _rankRepository;
    private readonly ISimilarityRepository _similarityRepository;

    public SummaryModel( ILogger<SummaryModel> logger, IRankRepository rankRepository, ISimilarityRepository similarityRepository )
    {
        _logger = logger;
        _rankRepository = rankRepository;
        _similarityRepository = similarityRepository;
    }

    public double Rank { get; set; }
    public double Similarity { get; set; }

    public void OnGet( string id )
    {
        _logger.LogDebug( id );

        TextId textId = new TextId( id );
        Rank = ParseDouble( _rankRepository.Get( new RankId( textId ) ) );
        Similarity = ParseDouble( _similarityRepository.Get( new SimilarityId( textId ) ) );
    }

    private double ParseDouble( string? value )
    {
        return Double.TryParse( value, CultureInfo.InvariantCulture, out double result )
            ? result
            : 0;
    }
}