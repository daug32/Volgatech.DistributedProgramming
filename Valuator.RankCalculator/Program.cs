using Valuator.Repositories.Redis;
using Infrastructure.Common;
using MessageBus.Nats;
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
            .ConfigureServices( ( context, collection ) => ConfigureServices( collection ) )
            .Build();

        return host.StartAsync();
    }

    private static IServiceCollection ConfigureServices( IServiceCollection serviceCollection )
    {
        serviceCollection
            .AddLogging( x => x.ClearProviders().AddConsole() )
            .AddRedisDatabase( new RedisConfigurationParser().FromEnvironment() )
            .AddNatsMessageBus( consumerRegistrator => consumerRegistrator.AddConsumers() )
            .AddHostedService<Application>();

        return serviceCollection;
    }
}