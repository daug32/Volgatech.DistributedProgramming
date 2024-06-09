using Valuator.Repositories.Redis;
using MessageBus.Nats;
using Valuator.Repositories.Redis.Configurations;

namespace Valuator;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddNatsMessageBus();
        
        var redisConfigurationParser = new RedisConfigurationParser();
        builder.Services.AddRedisDatabase( new RedisConfiguration
        {
            Mapper = redisConfigurationParser.GetMapperConfiguration( builder.Configuration )!,
            Shards = redisConfigurationParser.GetRegionsConfigurations()
        } );

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
}
