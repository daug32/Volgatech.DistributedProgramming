using Valuator.Domain.Regions;
using Valuator.Domain.ValueObjects;

namespace Valuator.Repositories.Interfaces.Shards;

public interface IRegionSearcher
{
    Region? Search( TextId textId );
}