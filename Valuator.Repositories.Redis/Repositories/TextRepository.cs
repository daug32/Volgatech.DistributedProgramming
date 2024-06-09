using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Domain.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class TextRepository( IDatabase database, IServer server ) : ITextRepository
{
    public void Add( TextId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        database.StringSet( key.ToString(), value );
    }

    public string? Get( TextId key ) => database.StringGet( key.ToString() );

    public List<TextId> GetAllTexts()
    {
        var rawKeys = server.Keys();

        return rawKeys
            .Where( key => TextId.IsTextId( key! ) )
            .Select( key => new TextId( key! ) )
            .ToList();
    }

    public bool Contains( TextId textId ) => database.KeyExists( textId.ToString() );
}