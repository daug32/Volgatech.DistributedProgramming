using Caches.Interfaces;
using Valuator.Domain.Regions;

namespace Caches.Extensions.Implementation;

internal class ShardSearcher : IShardSearcher
{
    private readonly ICacheServiceFactory _cacheServiceFactory;

    public ShardSearcher( ICacheServiceFactory cacheServiceFactory )
    {
        _cacheServiceFactory = cacheServiceFactory;
    }

    public ICacheService? Find( IShardKey cacheKey )
    {
        foreach ( Region region in Region.GetAllRegions() )
        {
            ICacheService cacheService = _cacheServiceFactory.CreateForRegion( region );
            if ( !cacheService.HasKey( cacheKey.ToCacheKey() ) )
            {
                continue;
            }

            return cacheService;
        }

        return null;
    }
}