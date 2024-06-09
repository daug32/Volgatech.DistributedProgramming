using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Interfaces;

public interface ITextRepository : IShardRepository
{
    void Add( TextId key, string value );
    string? Get( TextId key );
    List<TextId> GetAllTexts();
    bool Contains( TextId textId );
}