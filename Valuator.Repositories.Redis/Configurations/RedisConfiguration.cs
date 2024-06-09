namespace Valuator.Repositories.Redis.Configurations;

public class RedisConfiguration
{
    public RedisConnectionConfiguration Mapper { get; set; }
    public Dictionary<string, RedisConnectionConfiguration> Shards { get; set; }
}