using System.Net;
using System.Net.Sockets;
using Sockets.Implementation;

namespace Sockets.Connectors;

public class ConnectionCreator
{
    private readonly IpAddressCreator _ipAddressCreator = new();
    
    public Socket ConnectToServer( string host, int port )
    {
        IPAddress ipAddress = _ipAddressCreator.Create( host );
        
        Socket sender = new Socket( ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        sender.Connect( ipAddress, port );

        return sender;
    }
}