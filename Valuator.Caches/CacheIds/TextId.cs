using Caches.Interfaces;

namespace Valuator.Caches.CacheIds;

public class TextId
{
    public readonly string Value;

    public TextId( IndexModelId indexedModelId )
    {
        Value = $"TEXT-{indexedModelId}";
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}