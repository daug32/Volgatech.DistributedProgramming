using Valuator.Domain.Regions;

namespace Valuator.Repositories.Interfaces.Shards;

public interface IShardedRepositoryCreator<out TRepository>
    where TRepository : IShardRepository
{
    public TRepository Create( Region region );
}