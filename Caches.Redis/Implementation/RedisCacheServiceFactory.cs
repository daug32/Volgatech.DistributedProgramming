using Caches.Interfaces;
using StackExchange.Redis;
using Valuator.Domain.Regions;

namespace Caches.Redis.Implementation;

internal class RedisCacheServiceFactory : ICacheServiceFactory
{
    private readonly RedisConfiguration _redisConfiguration;

    private readonly Dictionary<Region, RedisConnection> _connectionToRegion = new();

    public RedisCacheServiceFactory( RedisConfiguration redisConfiguration )
    {
        _redisConfiguration = redisConfiguration;
    }

    public ICacheService CreateForRegion( Region region )
    {
        if ( !_redisConfiguration.Shards.ContainsKey( region.Value ) )
        {
            throw new ArgumentException( $"Shard configuration was not found for the region. Region: {region}" );
        }

        RedisShardConfiguration shardConfiguration = _redisConfiguration.Shards[region.Value];
        
        if ( !_connectionToRegion.ContainsKey( region ) )
        {
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect( shardConfiguration.HostName );
            
            _connectionToRegion[region] = new RedisConnection( 
                connectionMultiplexer.GetServer( shardConfiguration.HostName, shardConfiguration.Port ),
                connectionMultiplexer.GetDatabase() );
        }

        RedisConnection connection = _connectionToRegion[region];

        return new RedisCacheService( connection.Database, connection.Server );
    }
}