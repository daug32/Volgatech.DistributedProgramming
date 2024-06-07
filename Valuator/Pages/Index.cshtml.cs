using System.Globalization;
using Caches.Interfaces;
using MessageBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.CacheIds;
using Valuator.Domain.Countries;
using Valuator.Domain.Regions;
using Valuator.MessageBus;
using Valuator.MessageBus.DTOs;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICacheService _cacheService;
    private readonly IMessagePublisher _messagePublisher;

    public List<Country> Countries = Country.GetAllCountries();

    public IndexModel( ILogger<IndexModel> logger, ICacheService cacheService, IMessagePublisher messagePublisher )
    {
        _logger = logger;
        _cacheService = cacheService;
        _messagePublisher = messagePublisher;
    }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost( string text, int countryIndex )
    {
        _logger.LogDebug( $"{text}\n{countryIndex}" );

        Region region = Countries[countryIndex].ToRegion();
        var indexedModelId = IndexModelId.New();
        
        var textId = new TextId( indexedModelId );
        _cacheService.Add( textId.ToCacheKey(), text );
            
        _messagePublisher.Publish( Messages.CalculateRankRequest, indexedModelId.ToString() );

        int similarity = CalculateSimilarity( text );
        _cacheService.Add( new SimilarityId( indexedModelId ).ToCacheKey(), similarity.ToString( CultureInfo.InvariantCulture ) );
        _messagePublisher.Publish( Messages.SimilarityCalculatedNotification, new SimilarityCalculatedNotificationDto
        {
            Similarity = similarity,
            TextId = textId.ToString()
        } );

        return Redirect( $"summary?id={indexedModelId}" );
    }

    private int CalculateSimilarity( string text )
    {
        var keys = _cacheService.GetAllKeys();

        bool hasSameText = keys.Any( key => _cacheService
            .Get( key )
            !.Equals( text, StringComparison.InvariantCultureIgnoreCase ) );

        return hasSameText
            ? 1
            : 0;
    }
}