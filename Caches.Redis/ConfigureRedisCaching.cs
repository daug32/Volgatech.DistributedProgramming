using Caches.Interfaces;
using Caches.Redis.Implementation;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Caches.Redis;

public static class ConfigureRedisCaching 
{
    public static IServiceCollection AddRedisCache(
        this IServiceCollection services,
        RedisConfiguration redisConfiguration )
    {
        services.AddSingleton( _ => redisConfiguration );
        
        ConnectionMultiplexer connection = ConnectionMultiplexer.Connect( redisConfiguration.HostName );
        services.AddTransient<IDatabase>( _ => connection.GetDatabase() );
        services.AddTransient<IServer>( _ => connection.GetServer( redisConfiguration.HostName, redisConfiguration.Port ) );
        services.AddTransient<ICacheService, RedisCacheService>();

        return services;
    }  
}