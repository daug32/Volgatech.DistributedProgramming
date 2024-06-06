namespace Valuator.Models;

public class TextId
{
    public readonly string Value;

    public TextId( IndexModelId indexedModelId )
    {
        Value = $"TEXT-{indexedModelId}";
    }

    public override string ToString() => Value;
}