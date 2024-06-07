using Caches.Extensions;
using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class ShardKey : IShardKey
{
    public readonly string Value;

    public ShardKey( TextId textId )
    {
        Value = $"SHARD-{textId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}