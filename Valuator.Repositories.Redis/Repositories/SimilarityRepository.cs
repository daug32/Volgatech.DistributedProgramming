using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Caches.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class SimilarityRepository : ISimilarityRepository
{
    private readonly IDatabase _database;

    public SimilarityRepository( IDatabase database )
    {
        _database = database;
    }

    public void Add( SimilarityId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        _database.StringSet( key.ToString(), value );
    }

    public string? Get( SimilarityId key )
    {
        return _database.StringGet( key.ToString() );
    }
}