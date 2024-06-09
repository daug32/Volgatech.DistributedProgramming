using Microsoft.Extensions.Configuration;
using Valuator.Domain.Regions;

namespace Valuator.Repositories.Redis;

// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    public static RedisConfiguration GetRedisConfiguration( this IConfiguration configuration )
    {
        RedisConfiguration? redisConfiguration = configuration
            .GetSection( "Redis" )
            .Get<RedisConfiguration>();

        return ValidateRedisConfiguration( redisConfiguration );
    }

    private static RedisConfiguration ValidateRedisConfiguration( RedisConfiguration? configuration )
    {
        if ( configuration is null )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration )} can not be null" );
        }

        foreach ( RedisShardConfiguration redisShardConfiguration in configuration.Shards.Values )
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

        foreach ( Region region in Region.GetAllRegions() )
        {
            if ( !configuration.Shards.ContainsKey( region.Value.ToUpper() ) )
            {
                throw new ArgumentException( $"Redis configuration must have configs for each region. Region without config: {region}" );
            }
        }

        return configuration;
    }
}