using Valuator.Domain.ValueObjects;

namespace Valuator.Repositories.Interfaces;

public interface ITextRepository
{
    void Add( TextId key, string value );
    string? Get( TextId key );
    List<TextId> GetAllTexts();   
}