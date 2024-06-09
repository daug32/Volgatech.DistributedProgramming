using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Caches.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class RankRepository : IRankRepository
{
    private readonly IDatabase _database;

    public RankRepository( IDatabase database )
    {
        _database = database;
    }

    public void Add( RankId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        _database.StringSet( key.ToString(), value );
    }

    public string? Get( RankId key )
    {
        return _database.StringGet( key.ToString() );
    }
}