using Valuator.Domain.Regions;
using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class RegionSearcher( IShardedRepositoryCreator<ITextRepository> shardedRepositoryCreator ) : IRegionSearcher
{
    public Region? Search( TextId textId )
    {
        foreach ( Region region in Region.GetAllRegions() )
        {
            ITextRepository repository = shardedRepositoryCreator.Create( region );
            if ( repository.Contains( textId ) )
            {
                return region;
            }
        }

        return null;
    }
}