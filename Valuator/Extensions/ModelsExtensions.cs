using Caches.Interfaces;
using Valuator.Models;

namespace Valuator.Extensions;

public static class ModelsExtensions
{
    public static CacheKey ToCacheKey( this TextId id ) => new CacheKey( id.ToString() );
    public static CacheKey ToCacheKey( this RankId id ) => new CacheKey( id.ToString() );
    public static CacheKey ToCacheKey( this SimilarityId id ) => new CacheKey( id.ToString() );
}