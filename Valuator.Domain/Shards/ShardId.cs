using Valuator.Domain.ValueObjects;

namespace Valuator.Domain.Shards;

public record ShardId
{
    public readonly string Value;

    public ShardId( TextId id )
    {
        Value = $"SHARD-{id}";
    }

    public override string ToString() => Value;
}