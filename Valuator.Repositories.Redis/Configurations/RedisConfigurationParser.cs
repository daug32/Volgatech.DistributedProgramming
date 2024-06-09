using Microsoft.Extensions.Configuration;
using Valuator.Domain.Regions;
using Valuator.Repositories.Redis.Configurations.Implementation;

namespace Valuator.Repositories.Redis.Configurations;

public class RedisConfigurationParser
{
    public RedisConfiguration FromAppConfiguration( IConfiguration appConfiguration )
    {
        var configuration = appConfiguration
            .GetSection( "Redis" )
            .Get<RedisConfiguration>();

        return RedisConfigurationValidator.Validate( configuration );
    }

    public RedisConfiguration FromEnvironment()
    {
        var regionsToHostPort = Region
            .GetAllRegions()
            .Select( region => ( Region: region, Value: Environment.GetEnvironmentVariable( $"DB_{region.Value}" ) ) )
            .Where( pair => !String.IsNullOrWhiteSpace( pair.Value ) )
            .ToDictionary( pair => pair.Region, pair => pair.Value! );

        return FromDictionary( regionsToHostPort );
    }

    public RedisConfiguration FromDictionary( Dictionary<Region, string> regionsToHostPort )
    {
        var shardsConfigurations = new Dictionary<string, RedisShardConfiguration>();

        foreach ( ( Region region, string hostPort ) in regionsToHostPort )
        {
            string host = hostPort.Split( ':' )[0];
            if ( !Int32.TryParse( hostPort.Split( ':' )[1], out int port ) )
            {
                throw new ArgumentException( $"Can't parse port from string. String: {hostPort}, Region: {region}" );
            }

            shardsConfigurations.Add(
                region.Value,
                new RedisShardConfiguration
                {
                    Port = port,
                    HostName = host
                } );
        }

        return RedisConfigurationValidator.Validate( new RedisConfiguration
        {
            Shards = shardsConfigurations
        } );
    }
}