using Caches.Extensions;
using Caches.Redis;
using Infrastructure.Common;
using MessageBus.Nats;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valuator.Domain.Regions;
using Valuator.RankCalculator.Consumers;

namespace Valuator.RankCalculator;

public class Program
{
    public static Task Main()
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration( builder => builder.AddCommonConfiguration() )
            .ConfigureServices( ( context, collection ) => ConfigureServices( collection ) )
            .Build();

        return host.StartAsync();
    }

    private static IServiceCollection ConfigureServices( IServiceCollection serviceCollection )
    {
        serviceCollection
            .AddLogging( x => x.ClearProviders().AddConsole() )
            .AddRedisCache( GetRedsShardsConfigurations() )
            .AddShardSearching()
            .AddNatsMessageBus( consumerRegistrator => consumerRegistrator.AddConsumers() )
            .AddHostedService<Application>();

        return serviceCollection;
    }

    private static RedisConfiguration GetRedsShardsConfigurations()
    {
        List<string> allRegions = Region
            .GetAllRegions()
            .Select( x => x.Value )
            .ToList();

        Dictionary<string, string> regionsToHostPort = EnvironmentHelper.GetRedisConnections( allRegions );
        foreach ( string region in allRegions )
        {
            if ( regionsToHostPort.ContainsKey( region ) )
            {
                continue;
            }

            throw new ArgumentException( $"Not all regions have its own reds configuration. Region without configuration: {region}" );
        }

        return new RedisConfigurationParser().FromDictionary( regionsToHostPort );
    }
}