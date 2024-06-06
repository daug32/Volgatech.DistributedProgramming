namespace Valuator.Models;

public class SimilarityId
{
    public readonly string Value;

    public SimilarityId( IndexModelId indexedModelId )
    {
        Value = $"SIMILARITY-{indexedModelId}";
    }

    public override string ToString() => Value;
}