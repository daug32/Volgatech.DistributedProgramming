using StackExchange.Redis;
using Valuator.Domain.Regions;
using Valuator.Domain.Shards;
using Valuator.Repositories.Interfaces.Shards;
using Valuator.Repositories.Redis.Configurations;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class ShardMap : IShardMap
{
    private readonly IDatabase _database;

    public ShardMap( RedisConfiguration redisConfiguration )
    {
        RedisConnectionConfiguration mapperConfiguration = redisConfiguration.Mapper;
        ConnectionMultiplexer connection = ConnectionMultiplexer.Connect( $"{mapperConfiguration.HostName}:{mapperConfiguration.Port}" );
        _database = connection.GetDatabase();
    }

    public void Add( ShardId shardId, Region region ) => _database.StringSet( shardId.ToString(), region.Value );

    public Region? Search( ShardId textId )
    {
        string? value = _database.StringGet( textId.ToString() );
        return value == null
            ? null
            : new Region( value );
    }
}