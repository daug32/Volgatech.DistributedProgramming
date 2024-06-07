namespace Caches.Interfaces;

public interface ICacheService
{
    void Add( CacheKey key, string value );

    string? Get( CacheKey key );

    bool HasKey( CacheKey key );

    List<CacheKey> GetAllKeys();
}