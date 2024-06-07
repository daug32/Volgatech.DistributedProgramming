using Caches.Interfaces;

namespace Caches.Extensions;

public interface IShardSearcher
{
    ICacheService? Find( IShardKey cacheKey );
}