using Valuator.Domain.Regions;

namespace Caches.Interfaces;

public interface ICacheServiceFactory
{
    ICacheService CreateForRegion( Region region );
}