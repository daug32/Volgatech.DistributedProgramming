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
    private readonly ICacheServiceFactory _cacheServiceFactory;
    private readonly IMessagePublisher _messagePublisher;

    public List<Country> Countries = Country.GetAllCountries();

    public IndexModel( ILogger<IndexModel> logger, ICacheServiceFactory cacheServiceFactory, IMessagePublisher messagePublisher )
    {
        _logger = logger;
        _cacheServiceFactory = cacheServiceFactory;
        _messagePublisher = messagePublisher;
    }

    public void OnGet()
    {
        
    }

    public IActionResult OnPost( string text, int countryIndex )
    {
        _logger.LogDebug( text );
        ICacheService cacheService = GetCacheService( Countries[countryIndex].ToRegion() );
        
        var indexedModelId = IndexModelId.New();
        
        var textId = new TextId( indexedModelId );
        int similarity = CalculateSimilarity( text, cacheService );
        
        cacheService.Add( 
            textId.ToCacheKey(), 
            text );
        cacheService.Add( 
            new SimilarityId( indexedModelId ).ToCacheKey(), 
            similarity.ToString( CultureInfo.InvariantCulture ) );

        _messagePublisher.Publish( Messages.CalculateRankRequest, indexedModelId.ToString() );
        
        _messagePublisher.Publish( Messages.SimilarityCalculatedNotification, new SimilarityCalculatedNotificationDto
        {
            Similarity = similarity,
            TextId = textId.ToString()
        } );

        return Redirect( $"summary?id={indexedModelId}" );
    }

    private int CalculateSimilarity( string text, ICacheService cacheService )
    {
        var keys = cacheService.GetAllKeys();

        bool hasSameText = false;
        foreach ( CacheKey key in keys )
        {
            if ( cacheService.Get( key )!.Equals( text, StringComparison.InvariantCultureIgnoreCase ) )
            {
                hasSameText = true;
                break;
            }
        }

        return hasSameText
            ? 1
            : 0;
    }

    private ICacheService GetCacheService( Region region )
    {
        return _cacheServiceFactory.CreateForRegion( region );
    }
}