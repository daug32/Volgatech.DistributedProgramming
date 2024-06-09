namespace FluentAssertions;

public static class FluentAssertions
{
    public static string ThrowIfNullOrEmpty( this string str ) => String.IsNullOrWhiteSpace( str )
        ? throw new ArgumentException( "Value must noe be null or empty" )
        : str;
}