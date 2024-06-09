using Valuator.Domain.Regions;

namespace Valuator.Domain.Countries;

public static class CountryToRegionMap
{
    private static readonly Dictionary<Country, Region> _map = BuildMap();

    public static Region ToRegion( this Country country )
    {
        return _map.TryGetValue( country, out Region? region )
            ? region
            : Region.Other;
    }

    private static Dictionary<Country, Region> BuildMap()
    {
        return new Dictionary<Country, Region>
        {
            { Country.Russia, Region.Russia },

            { Country.France, Region.EuropeUnion },
            { Country.Germany, Region.EuropeUnion },

            { Country.USA, Region.Other },
            { Country.India, Region.Other }
        };
    }
}