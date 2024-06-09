using Valuator.Domain.Regions;

namespace Valuator.Repositories.Redis.Configurations.Implementation;

internal static class RedisConfigurationValidator
{
    public static RedisConfiguration Validate( RedisConfiguration? configuration )
    {
        if ( configuration is null )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration )} can not be null" );
        }

        Validate( configuration.Mapper );
        foreach ( RedisConnectionConfiguration redisShardConfiguration in configuration.Shards.Values )
        {
            Validate( redisShardConfiguration );
        }

        AssumeEachRegionHaveConfiguration( configuration );

        return configuration;
    }

    public static RedisConnectionConfiguration Validate( RedisConnectionConfiguration connectionConfiguration )
    {
        if ( String.IsNullOrWhiteSpace( connectionConfiguration.HostName ) )
        {
            throw new ArgumentException( $"{nameof( RedisConnectionConfiguration.HostName )} can not be null or whitespace" );
        }

        if ( connectionConfiguration.Port is < 1 or >= Int16.MaxValue )
        {
            throw new ArgumentException( $"{nameof( RedisConnectionConfiguration.Port )} is not in valid range. Expected: [1; {Int16.MaxValue}]" );
        }

        return connectionConfiguration;
    }

    private static void AssumeEachRegionHaveConfiguration( RedisConfiguration configuration )
    {
        var regionsWithoutConfiguration = new List<Region>();
        foreach ( Region region in Region.GetAllRegions() )
        {
            if ( !configuration.Shards.ContainsKey( region.Value ) )
            {
                regionsWithoutConfiguration.Add( region );
            }
        }

        if ( regionsWithoutConfiguration.Any() )
        {
            throw new ArgumentException(
                $"Redis configuration must have configs for each existing region. "
                + $"Regions without configurations: {String.Join( ", ", regionsWithoutConfiguration )}" );
        }
    }
}