using Caches.Extensions.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Caches.Extensions;

public static class ConfigureDependencies
{
    public static IServiceCollection AddShardSearching( this IServiceCollection services )
    {
        return services.AddScoped<IShardSearcher, ShardSearcher>();
    }
}