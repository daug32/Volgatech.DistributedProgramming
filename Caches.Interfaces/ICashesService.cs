namespace Caches.Interfaces;

public interface ICacheService
{
    void Add( CacheKey key, string value );

    string? Get( CacheKey key );

    List<CacheKey> GetAllKeys();
}