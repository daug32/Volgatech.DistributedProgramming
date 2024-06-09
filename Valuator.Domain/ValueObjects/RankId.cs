namespace Valuator.Domain.ValueObjects;

public class RankId
{
    public readonly string Value;

    public RankId( TextId id )
    {
        Value = $"RANK-{id}";
    }

    public override string ToString() => Value;
}