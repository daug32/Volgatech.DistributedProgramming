namespace Valuator.Domain.ValueObjects;

public record RankId
{
    public readonly string Value;

    public RankId( TextId id )
    {
        Value = $"RANK-{id}";
    }

    public override string ToString() => Value;
}