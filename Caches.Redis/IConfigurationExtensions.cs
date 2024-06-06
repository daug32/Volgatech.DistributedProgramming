using Microsoft.Extensions.Configuration;

namespace Caches.Redis;

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
        
        if ( String.IsNullOrWhiteSpace( configuration.HostName ) )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration.HostName )} can not be null or whitespace" );
        }

        if ( configuration.Port is < 1 or >= Int16.MaxValue )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration.Port )} is not in valid range. Expected: [1; {Int16.MaxValue}]" );
        }
        
        return configuration;
    }
}