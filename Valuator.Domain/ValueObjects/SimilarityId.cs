namespace Valuator.Domain.ValueObjects;

public record SimilarityId
{
    public readonly string Value;

    public SimilarityId( TextId id )
    {
        Value = $"SIMILARITY-{id}";
    }

    public override string ToString() => Value;
}