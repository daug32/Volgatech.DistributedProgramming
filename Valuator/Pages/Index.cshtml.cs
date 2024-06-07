using System.Globalization;
using Caches.Extensions;
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
        var region = Countries[countryIndex].ToRegion();
        ICacheService cacheService = GetCacheService( region );

        var indexedModelId = IndexModelId.New();
        
        var textId = new TextId( indexedModelId );

        var shardId = new ShardKey( textId ).ToCacheKey();
        var shardRegion = region.ToString();
        cacheService.Add( shardId, shardRegion );
        
        var similarityId = new SimilarityId( indexedModelId );
        int similarity = CalculateSimilarity( text, cacheService );
        cacheService.Add( similarityId.ToCacheKey(), similarity.ToString( CultureInfo.InvariantCulture ) );
        
        cacheService.Add( textId.ToCacheKey(), text );

        _messagePublisher.Publish( Messages.CalculateRankRequest, indexedModelId.ToString() );

        _messagePublisher.Publish( 
            Messages.SimilarityCalculatedNotification,
            new SimilarityCalculatedNotificationDto
            {
                Similarity = similarity,
                TextId = textId.ToString()
            } );

        return Redirect( $"summary?id={indexedModelId}" );
    }

    private int CalculateSimilarity( string text, ICacheService cacheService )
    {
        var keys = cacheService.GetAllKeys();

        var hasSameText = false;
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