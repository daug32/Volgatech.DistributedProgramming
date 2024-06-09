namespace Valuator.Repositories.Redis.Configurations;

public class RedisConfiguration
{
    public Dictionary<string, RedisShardConfiguration> Shards { get; set; }
}