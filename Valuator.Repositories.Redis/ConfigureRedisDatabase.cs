using Valuator.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Valuator.Repositories.Interfaces.Shards;
using Valuator.Repositories.Redis.Configurations;
using Valuator.Repositories.Redis.Repositories.Shards;

namespace Valuator.Repositories.Redis;

public static class ConfigureRedisDatabase
{
    public static IServiceCollection AddRedisDatabase(
        this IServiceCollection services,
        RedisConfiguration redisConfiguration )
    {
        services.AddSingleton( _ => redisConfiguration );

        services.AddTransient<IRegionSearcher, RegionSearcher>();

        services.AddTransient<IShardedRepositoryCreator<ITextRepository>, ShardedRepositoryCreator>();
        services.AddTransient<IShardedRepositoryCreator<ISimilarityRepository>, ShardedRepositoryCreator>();
        services.AddTransient<IShardedRepositoryCreator<IRankRepository>, ShardedRepositoryCreator>();

        return services;
    }
}