namespace Caches.Interfaces;

public class CacheKey
{
    public readonly string Value;

    public CacheKey( string value )
    {
        Value = String.IsNullOrWhiteSpace( value )
            ? throw new ArgumentException( "Cache key can not be null or empty" )
            : value;
    }
}