using Valuator.Domain.Regions;
using Valuator.Domain.ValueObjects;
using Valuator.Repositories.Interfaces;
using Valuator.Repositories.Interfaces.Shards;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class RegionSearcher : IRegionSearcher
{
    private readonly IShardedRepositoryCreator<ITextRepository> _shardedRepositoryCreator;

    public RegionSearcher( IShardedRepositoryCreator<ITextRepository> shardedRepositoryCreator )
    {
        _shardedRepositoryCreator = shardedRepositoryCreator;
    }

    public Region? Search( TextId textId )
    {
        foreach ( Region region in Region.GetAllRegions() )
        {
            ITextRepository repository = _shardedRepositoryCreator.Create( region );
            if ( repository.Contains( textId ) )
            {
                return region;
            }
        }

        return null;
    }
}