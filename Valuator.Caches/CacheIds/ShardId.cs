using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class ShardId
{
    public readonly string Value;

    public ShardId( IndexModelId indexedModelId )
    {
        Value = $"SHARD-{indexedModelId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}