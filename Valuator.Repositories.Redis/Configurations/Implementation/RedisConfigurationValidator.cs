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

        foreach ( RedisShardConfiguration redisShardConfiguration in configuration.Shards.Values )
        {
            ValidateShardConfiguration( redisShardConfiguration );
        }

        AssumeEachRegionHaveConfiguration( configuration );

        return configuration;
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

    private static void ValidateShardConfiguration( RedisShardConfiguration redisShardConfiguration )
    {
        if ( String.IsNullOrWhiteSpace( redisShardConfiguration.HostName ) )
        {
            throw new ArgumentException( $"{nameof( RedisShardConfiguration.HostName )} can not be null or whitespace" );
        }

        if ( redisShardConfiguration.Port is < 1 or >= Int16.MaxValue )
        {
            throw new ArgumentException( $"{nameof( RedisShardConfiguration.Port )} is not in valid range. Expected: [1; {Int16.MaxValue}]" );
        }
    }
}