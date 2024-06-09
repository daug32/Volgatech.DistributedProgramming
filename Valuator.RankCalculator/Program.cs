using Valuator.Repositories.Redis;
using Infrastructure.Common;
using MessageBus.Nats;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valuator.RankCalculator.Consumers;
using Valuator.Repositories.Redis.Configurations;

namespace Valuator.RankCalculator;

public class Program
{
    public static Task Main()
    {
        IHost host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration( builder => builder.AddCommonConfiguration() )
            .ConfigureServices( ( context, collection ) => ConfigureServices( collection, context.Configuration ) )
            .Build();

        return host.StartAsync();
    }

    private static IServiceCollection ConfigureServices( IServiceCollection serviceCollection, IConfiguration configuration )
    {
        var redisConfigurationParser = new RedisConfigurationParser();
        
        serviceCollection
            .AddLogging( x => x.ClearProviders().AddConsole() )
            .AddRedisDatabase( new RedisConfiguration()
            {
                Mapper = redisConfigurationParser.GetMapperConfiguration( configuration )!,
                Shards = redisConfigurationParser.GetRegionsConfigurations()
            } )
            .AddNatsMessageBus( consumerRegistrator => consumerRegistrator.AddConsumers() )
            .AddHostedService<Application>();

        return serviceCollection;
    }
}