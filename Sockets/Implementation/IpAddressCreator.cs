using System.Net;

namespace Sockets.Implementation;

internal class IpAddressCreator
{
    public IPAddress Create( string host )
    {
        return IPAddress.Parse( host == "localhost"
            ? "127.0.0.1"
            : host );
    }
}