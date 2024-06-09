using Valuator.Domain.Regions;
using Valuator.Domain.Shards;

namespace Valuator.Repositories.Interfaces.Shards;

public interface IShardMap
{
    void Add( ShardId shardId, Region region );
    Region? Search( ShardId textId );
}