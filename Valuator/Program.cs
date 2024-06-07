using Caches.Redis;
using Infrastructure.Common;
using MessageBus.Nats;
using Valuator.Caches.ShardSearching;
using Valuator.Domain.Regions;

namespace Valuator;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddNatsMessageBus();
        builder.Services.AddRedisCache( GetRedsShardsConfigurations() );
        builder.Services.AddShardSearching();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
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
