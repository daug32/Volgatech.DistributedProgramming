using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Interfaces;

public interface IRankRepository : IShardRepository
{
    void Add( RankId key, string value );
    string? Get( RankId key );
}