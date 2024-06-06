using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class RankId
{
    public readonly string Value;

    public RankId( IndexModelId indexedModelId )
    {
        Value = $"RANK-{indexedModelId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}