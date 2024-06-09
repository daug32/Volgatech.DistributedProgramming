using Valuator.Domain.ValueObjects;

namespace Valuator.Repositories.Interfaces;

public interface ISimilarityRepository
{
    void Add( SimilarityId key, string value );
    string? Get( SimilarityId key );
}