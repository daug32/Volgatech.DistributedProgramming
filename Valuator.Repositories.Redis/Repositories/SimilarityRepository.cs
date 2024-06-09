using Valuator.Repositories.Interfaces;
using StackExchange.Redis;
using Valuator.Domain.ValueObjects;

namespace Valuator.Repositories.Redis.Repositories;

internal class SimilarityRepository( IDatabase database ) : ISimilarityRepository
{
    public void Add( SimilarityId key, string value )
    {
        if ( String.IsNullOrWhiteSpace( value ) )
        {
            throw new ArgumentException( $"{nameof( value )} can not be null or empty" );
        }

        database.StringSet( key.ToString(), value );
    }

    public string? Get( SimilarityId key )
    {
        return database.StringGet( key.ToString() );
    }
}