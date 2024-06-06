using System.Globalization;
using Caches.Interfaces;
using MessageBus.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Valuator.Caches.CacheIds;

namespace Valuator.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ICacheService _cacheService;
    private readonly IMessagePublisher _messagePublisher;

    public IndexModel( ILogger<IndexModel> logger, ICacheService cacheService, IMessagePublisher messagePublisher )
    {
        _logger = logger;
        _cacheService = cacheService;
        _messagePublisher = messagePublisher;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost( string text )
    {
        _logger.LogDebug( text );

        var id = IndexModelId.New();
        int similarity = CalculateSimilarity( text );
        _messagePublisher.Publish( Messages.Messages.CalculateRankMessage, id.ToString() );

        _cacheService.Add( new TextId( id ).ToCacheKey(), text );
        _cacheService.Add( new SimilarityId( id ).ToCacheKey(), similarity.ToString( CultureInfo.InvariantCulture ) );

        return Redirect( $"summary?id={id}" );
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