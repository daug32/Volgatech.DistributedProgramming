namespace Caches.Redis;

public class RedisConfigurationParser
{
    public RedisConfiguration FromDictionary( Dictionary<string, string> regionsToHostPort )
    {
        var shardsConfigurations = new Dictionary<string, RedisShardConfiguration>();
        
        foreach ( ( string region, string hostPort ) in regionsToHostPort )
        {
            string host = hostPort.Split( ':' )[0];
            if ( !Int32.TryParse( hostPort.Split( ':' )[1], out int port ) )
            {
                throw new ArgumentException( $"Can't parse port from string. String: {hostPort}, Region: {region}" );
            }
            
            shardsConfigurations.Add(
                region,
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