namespace Caches.Redis;

public class RedisConfiguration
{
    public Dictionary<string, RedisShardConfiguration> Shards { get; set; }
}