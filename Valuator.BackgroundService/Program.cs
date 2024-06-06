using Caches.Redis;
using Infrastructure.Common;
using MessageBus.Nats;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Valuator.BackgroundService.Consumers;

namespace Valuator.BackgroundService;

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
        serviceCollection
            .AddLogging( x => x.ClearProviders().AddConsole() )
            .AddRedisCache( configuration.GetRedisConfiguration() )
            .AddNatsMessageBus( builder => builder
                .AddConsumerForMessage<CalculateRankMessageConsumer>( Messages.Messages.CalculateRankMessage ) )
            .AddConsumers()
            .AddHostedService<Application>();

        return serviceCollection;
    }
}