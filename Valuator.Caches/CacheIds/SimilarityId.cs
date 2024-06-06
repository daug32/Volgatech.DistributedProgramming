using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class SimilarityId
{
    public readonly string Value;

    public SimilarityId( IndexModelId indexedModelId )
    {
        Value = $"SIMILARITY-{indexedModelId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}