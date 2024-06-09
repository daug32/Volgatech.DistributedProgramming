namespace Valuator.Domain.ValueObjects;

public record TextId
{
    public readonly string Value;

    public static TextId New() => new( Guid.NewGuid().ToString() );

    public TextId( string id )
    {
        if ( !id.StartsWith( "TEXT" ) )
        {
            id = $"TEXT-{id}";
        }

        Value = id;
    }

    public static bool IsTextId( string value ) => value.StartsWith( "TEXT-" );

    public virtual bool Equals( TextId? other ) => other is not null && other.Value.Equals( Value );

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}