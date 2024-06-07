using Caches.Interfaces;

namespace Valuator.Caches.ShardSearching;

public interface IShardSearcher
{
    ICacheService? Find( CacheKey cacheKey );
}