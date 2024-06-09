using System.Net;
using System.Net.Sockets;
using Sockets.Implementation;

namespace Sockets.Connectors;

public class ConnectionCreator
{
    private readonly IPAddress _host;
    private readonly int _port;

    public ConnectionCreator( string host, int port )
    {
        _host = new IpAddressCreator().Create( host );
        _port = port;
    }

    public Socket CreateConnection()
    {
        var sender = new Socket( _host.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        sender.Connect( _host, _port );

        return sender;
    }
}