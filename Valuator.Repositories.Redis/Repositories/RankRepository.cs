using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Caches.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class RankRepository( IDatabase database ) : IRankRepository
{
    public void Add( RankId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        database.StringSet( key.ToString(), value );
    }

    public string? Get( RankId key )
    {
        return database.StringGet( key.ToString() );
    }
}