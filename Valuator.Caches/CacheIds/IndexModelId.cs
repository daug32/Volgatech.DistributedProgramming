using Caches.Interfaces;
using FluentAssertions;

namespace Valuator.Caches.CacheIds;

public class IndexModelId
{
    public readonly string Value;

    public static IndexModelId New() => new IndexModelId( Guid.NewGuid() );

    public IndexModelId( Guid value ) : this( value.ToString() )
    {
    }

    public IndexModelId( string value )
    {
        Value = value.ThrowIfNullOrEmpty();
    }

    public override string ToString() => Value;

    public CacheKey ToCacheKey() => new( Value );
}