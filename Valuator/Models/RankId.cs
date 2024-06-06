namespace Valuator.Models;

public class RankId 
{
    public readonly string Value;

    public RankId( IndexModelId indexedModelId )
    {
        Value = $"RANK-{indexedModelId}";
    }

    public override string ToString() => Value;
}