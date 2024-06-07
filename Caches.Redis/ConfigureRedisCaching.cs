using Caches.Interfaces;
using Caches.Redis.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Caches.Redis;

public static class ConfigureRedisCaching 
{
    public static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        RedisConfiguration redisConfiguration )
    {
        ValidateRedisConfiguration( redisConfiguration );
        services.AddSingleton( _ => redisConfiguration );
        services.AddTransient<ICacheServiceFactory, RedisCacheServiceFactory>();

        return services;
    }  
    
    private static RedisConfiguration ValidateRedisConfiguration( RedisConfiguration? configuration )
    {
        if ( configuration is null )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration )} can not be null" );
        }
        
        foreach ( ( string region, RedisShardConfiguration shardConfiguration ) in configuration.Shards )
        {
            if ( String.IsNullOrWhiteSpace( region ) )
            {
                throw new ArgumentException( $"{nameof( region )} can not be null or empty" );
            }
            
            if ( String.IsNullOrWhiteSpace( shardConfiguration.HostName ) )
            {
                throw new ArgumentException( $"{nameof( RedisShardConfiguration.HostName )} can not be null or empty" );
            }

            if ( shardConfiguration.Port is < 1 or >= Int16.MaxValue )
            {
                throw new ArgumentException( $"{nameof( RedisShardConfiguration.Port )} is not in valid range. Expected: [1; {Int16.MaxValue}]" );
            }
        }
        
        return configuration;
    }
}