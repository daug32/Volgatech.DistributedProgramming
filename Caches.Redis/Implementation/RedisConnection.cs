using StackExchange.Redis;

namespace Caches.Redis.Implementation;

internal class RedisConnection
{
    public IServer Server;
    public IDatabase Database;

    public RedisConnection( IServer server, IDatabase database )
    {
        Server = server;
        Database = database;
    }
}