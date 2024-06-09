using StackExchange.Redis;

namespace Valuator.Repositories.Redis.Repositories.Shards;

internal class RedisConnection( IDatabase database, IServer server )
{
    public readonly IServer Server = server;
    public readonly IDatabase Database = database;
}