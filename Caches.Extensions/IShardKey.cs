using Caches.Interfaces;

namespace Caches.Extensions;

public interface IShardKey
{
    CacheKey ToCacheKey();
}