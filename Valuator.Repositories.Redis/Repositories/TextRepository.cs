using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Caches.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class TextRepository : ITextRepository
{
    private readonly IDatabase _database;
    private readonly IServer _server;

    public TextRepository( IDatabase database, IServer server )
    {
        _database = database;
        _server = server;
    }

    public void Add( TextId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        _database.StringSet( key.ToString(), value );
    }

    public string? Get( TextId key )
    {
        return _database.StringGet( key.ToString() );
    }

    public List<TextId> GetAllTexts()
    {
        var rawKeys = _server.Keys();

        return rawKeys
            .Where( key => key.ToString().StartsWith( "TEXT-" ) )
            .Select( key => new TextId( key ) )
            .ToList();
    }
}