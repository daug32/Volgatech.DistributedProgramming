using Valuator.Domain.Regions;

namespace Valuator.Repositories.Redis;

public class RedisConfiguration
{
    public Dictionary<string, RedisShardConfiguration> Shards { get; set; }
}

public class RedisShardConfiguration 
{
    public string HostName { get; set; }
    public int Port { get; set; }
}

public class RedisConfigurationParser
{
    public RedisConfiguration FromEnvironment()
    {
        Dictionary<Region, string> regionsToHostPort = Region
            .GetAllRegions()
            .Select( region => ( Region: region, Value: Environment.GetEnvironmentVariable( $"DB_{region.Value.ToUpper()}" ) ) )
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
                region.Value.ToUpper(),
                new RedisShardConfiguration()
                {
                    Port = port,
                    HostName = host
                } );
        }

        return new RedisConfiguration
        {
            Shards = shardsConfigurations
        };
    }
}