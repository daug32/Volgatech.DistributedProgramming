namespace Valuator.Models;

public class IndexModelId
{
    public readonly string Value;

    public static IndexModelId New() => new IndexModelId( Guid.NewGuid() );

    public IndexModelId( Guid value ) : this( value.ToString() )
    {
    }

    public IndexModelId( string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"Value must not be null or empty" );
        }
        
        Value = value;
    }

    public override string ToString() => Value;
}