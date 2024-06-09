using StackExchange.Redis;
using Valuator.Domain.Regions;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;
using Valuator.Repositories.Redis.Configurations;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class ShardedRepositoryCreator( RedisConfiguration redisConfiguration ) :
    IShardedRepositoryCreator<ITextRepository>,
    IShardedRepositoryCreator<ISimilarityRepository>,
    IShardedRepositoryCreator<IRankRepository>
{
    private readonly Dictionary<Region, RedisConnection> _connections = new();

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
            RedisConnectionConfiguration connectionConfiguration = redisConfiguration.Shards[region.Value];
            var multiplexer = ConnectionMultiplexer.Connect( $"{connectionConfiguration.HostName}:{connectionConfiguration.Port}" );

            _connections[region] = new RedisConnection(
                multiplexer.GetDatabase(),
                multiplexer.GetServer( connectionConfiguration.HostName, connectionConfiguration.Port ) );
        }

        return _connections[region];
    }
}