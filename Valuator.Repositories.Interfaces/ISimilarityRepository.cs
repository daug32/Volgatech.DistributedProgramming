using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Interfaces;

public interface ISimilarityRepository : IShardRepository
{
    void Add( SimilarityId key, string value );
    string? Get( SimilarityId key );
}