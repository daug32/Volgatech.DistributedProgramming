using StackExchange.Redis;
using Valuator.Domain.Regions;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class RedisConnection( IDatabase database, IServer server )
{
    public readonly IServer Server = server;
    public readonly IDatabase Database = database;
}

internal class ShardedRepositoryCreator :
    IShardedRepositoryCreator<ITextRepository>,
    IShardedRepositoryCreator<ISimilarityRepository>,
    IShardedRepositoryCreator<IRankRepository>
{
    private readonly RedisConfiguration _redisConfiguration;

    private readonly Dictionary<Region, RedisConnection> _connections = new();

    public ShardedRepositoryCreator( RedisConfiguration redisConfiguration )
    {
        _redisConfiguration = redisConfiguration;
    }

    ITextRepository IShardedRepositoryCreator<ITextRepository>.Create( Region region ) => 
        new TextRepository( GetConnection( region ).Database, GetConnection( region ).Server );

    ISimilarityRepository IShardedRepositoryCreator<ISimilarityRepository>.Create( Region region ) => 
        new SimilarityRepository( GetConnection( region ).Database );

    IRankRepository IShardedRepositoryCreator<IRankRepository>.Create( Region region ) => 
        new RankRepository( GetConnection( region ).Database );

    private RedisConnection GetConnection( Region region )
    {
        if ( !_connections.ContainsKey( region ) )
        {
            RedisShardConfiguration configuration = _redisConfiguration.Shards[region.Value.ToUpper()];
            ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect( $"{configuration.HostName}:{configuration.Port}" );

            _connections[region] = new RedisConnection(
                multiplexer.GetDatabase(),
                multiplexer.GetServer( configuration.HostName, configuration.Port ) );
        }

        return _connections[region];
    }
}