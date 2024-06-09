using System.Reflection;

namespace Valuator.Domain.Countries;

public partial class Country
{
    public static Country Russia => new( "RUS" );
    public static Country France => new( "FRA" );
    public static Country Germany => new( "DEU" );

    // ReSharper disable once InconsistentNaming
    public static Country USA => new( "USA" );
    public static Country India => new( "IND" );

    public static List<Country> GetAllCountries()
    {
        return typeof( Country )
            .GetProperties( BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty )
            .Where( x => x.PropertyType == typeof( Country ) )
            .Select( x => x.GetValue( null ) )
            .Cast<Country>()
            .ToList();
    }
}