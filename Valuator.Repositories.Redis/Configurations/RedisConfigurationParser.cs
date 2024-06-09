using Microsoft.Extensions.Configuration;
using Valuator.Domain.Regions;
using Valuator.Repositories.Redis.Configurations.Implementation;

namespace Valuator.Repositories.Redis.Configurations;

public class RedisConfigurationParser
{
    public RedisConnectionConfiguration? GetMapperConfiguration( IConfiguration appConfiguration )
    {
        return appConfiguration
            .GetSection( "Redis" )
            .Get<RedisConnectionConfiguration>();
    }

    public Dictionary<string, RedisConnectionConfiguration> GetRegionsConfigurations()
    {
        Dictionary<Region, string> regionsToHostPort = Region
            .GetAllRegions()
            .Select( region => ( Region: region, Value: Environment.GetEnvironmentVariable( $"DB_{region.Value}" ) ) )
            .Where( pair => !String.IsNullOrWhiteSpace( pair.Value ) )
            .ToDictionary( pair => pair.Region, pair => pair.Value! );

        return FromDictionary( regionsToHostPort );
    }

    private Dictionary<string, RedisConnectionConfiguration> FromDictionary( Dictionary<Region, string> regionsToHostPort )
    {
        var shardsConfigurations = new Dictionary<string, RedisConnectionConfiguration>();

        foreach ( ( Region region, string hostPort ) in regionsToHostPort )
        {
            string host = hostPort.Split( ':' )[0];
            if ( !Int32.TryParse( hostPort.Split( ':' )[1], out int port ) )
            {
                throw new ArgumentException( $"Can't parse port from string. String: {hostPort}, Region: {region}" );
            }

            shardsConfigurations.Add(
                region.Value,
                new RedisConnectionConfiguration
                {
                    Port = port,
                    HostName = host
                } );
        }

        return shardsConfigurations;
    }
}