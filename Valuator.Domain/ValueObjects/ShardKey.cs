namespace Valuator.Domain.ValueObjects;

public class ShardKey 
{
    public readonly string Value;

    public ShardKey( TextId id )
    {
        Value = $"SHARD-{id}";
    }

    public override string ToString() => Value;
}