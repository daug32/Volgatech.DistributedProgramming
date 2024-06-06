using Caches.Interfaces;
using StackExchange.Redis;

namespace Caches.Redis.Implementation;

internal class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly IServer _server;
    
    public RedisCacheService( IDatabase database, IServer server )
    {
        _database = database;
        _server = server;
    }

    public void Add( CacheKey key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        } 
        
        _database.StringSet( key.Value, value );
    }

    public string? Get( CacheKey key )
    {
        return _database.StringGet( key.Value );
    }

    public List<CacheKey> GetAllKeys()
    {
        var rawKeys = _server.Keys();
        
        return rawKeys
            .Select( key => new CacheKey( key ) )
            .ToList();
    }
}