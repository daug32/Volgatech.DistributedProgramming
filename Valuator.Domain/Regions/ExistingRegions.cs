using System.Reflection;

namespace Valuator.Domain.Regions;

public partial class Region
{
    public static Region Russia => new( "RUS" );
    public static Region EuropeUnion => new( "EU" );
    public static Region Other => new( "OTHER" );

    public static List<Region> GetAllRegions()
    {
        return typeof( Region )
            .GetProperties( BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty )
            .Where( x => x.PropertyType == typeof( Region ) )
            .Select( x => x.GetValue( null ) )
            .Cast<Region>()
            .ToList();
    }
}