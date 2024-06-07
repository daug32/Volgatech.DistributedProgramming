using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class ShardKey
{
    public readonly string Value;

    public ShardKey( TextId textId )
    {
        Value = $"SHARD-{textId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}