using Valuator.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Valuator.Repositories.Redis.Repositories;

namespace Valuator.Repositories.Redis;

public static class ConfigureRedisDatabase
{
    public static IServiceCollection AddRedisDatabase(
        this IServiceCollection services,
        RedisConfiguration redisConfiguration )
    {
        services.AddSingleton( _ => redisConfiguration );

        ConnectionMultiplexer connection = ConnectionMultiplexer.Connect( redisConfiguration.HostName );
        services.AddTransient<IDatabase>( _ => connection.GetDatabase() );
        services.AddTransient<IServer>( _ => connection.GetServer( redisConfiguration.HostName, redisConfiguration.Port ) );

        services.AddTransient<ITextRepository, TextRepository>();
        services.AddTransient<ISimilarityRepository, SimilarityRepository>();
        services.AddTransient<IRankRepository, RankRepository>();

        return services;
    }
}