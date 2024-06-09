using Microsoft.Extensions.Configuration;
using Valuator.Domain.Regions;

namespace Valuator.Repositories.Redis;

public class RedisConfigurationParser
{
    public RedisConfiguration FromAppConfiguration( IConfiguration appConfiguration )
    {
        RedisConfiguration? redisConfiguration = appConfiguration
            .GetSection( "Redis" )
            .Get<RedisConfiguration>();

        return ValidateRedisConfiguration( redisConfiguration );
    }
    
    public RedisConfiguration FromEnvironment()
    {
        Dictionary<Region, string> regionsToHostPort = Region
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
                new RedisShardConfiguration()
                {
                    Port = port,
                    HostName = host
                } );
        }

        var configuration = new RedisConfiguration
        {
            Shards = shardsConfigurations
        };

        return ValidateRedisConfiguration( configuration );
    }
    
    private static RedisConfiguration ValidateRedisConfiguration( RedisConfiguration? configuration )
    {
        if ( configuration is null )
        {
            throw new ArgumentException( $"{nameof( RedisConfiguration )} can not be null" );
        }

        foreach ( RedisShardConfiguration redisShardConfiguration in configuration.Shards.Values )
        {
            ValidateShardConfiguration( redisShardConfiguration );
        }

        AssumeEachRegionHaveConfiguration( configuration );

        return configuration;
    }

    private static void AssumeEachRegionHaveConfiguration( RedisConfiguration configuration )
    {
        var regionsWithoutConfiguration = new List<Region>();
        foreach ( Region region in Region.GetAllRegions() )
        {
            if ( !configuration.Shards.ContainsKey( region.Value ) )
            {
                regionsWithoutConfiguration.Add( region );
            }
        }

        if ( regionsWithoutConfiguration.Any() )
        {
            throw new ArgumentException( 
                $"Redis configuration must have configs for each existing region. "
                + $"Regions without configurations: {String.Join( ", ", regionsWithoutConfiguration )}" );
        }
    }

    private static void ValidateShardConfiguration( RedisShardConfiguration redisShardConfiguration )
    {
        if ( String.IsNullOrWhiteSpace( redisShardConfiguration.HostName ) )
        {
            throw new ArgumentException( $"{nameof( RedisShardConfiguration.HostName )} can not be null or whitespace" );
        }

        if ( redisShardConfiguration.Port is < 1 or >= Int16.MaxValue )
        {
            throw new ArgumentException( $"{nameof( RedisShardConfiguration.Port )} is not in valid range. Expected: [1; {Int16.MaxValue}]" );
        }
    }
}