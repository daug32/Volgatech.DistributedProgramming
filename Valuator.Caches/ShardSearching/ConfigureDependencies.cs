using Microsoft.Extensions.DependencyInjection;
using Valuator.Caches.ShardSearching.Implementation;

namespace Valuator.Caches.ShardSearching;

public static class ConfigureDependencies
{
    public static IServiceCollection AddShardSearching( this IServiceCollection services )
    {
        return services.AddScoped<IShardSearcher, ShardSearcher>();
    }
}