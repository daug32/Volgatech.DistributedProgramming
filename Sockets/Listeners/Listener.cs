using System.Net;
using System.Net.Sockets;
using Sockets.Implementation;
using Sockets.Models;

namespace Sockets.Listeners;

public class Listener
{
    private static readonly IpAddressCreator _ipAddressCreator = new();
    private static readonly Serializer _serializer = new();

    public void Listen(
        string host,
        int port,
        Func<Request?, Response> onDataReceived,
        CancellationToken token )
    {
        IPAddress ipAddress = _ipAddressCreator.Create( host );

        using var socket = new Socket( ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        socket.Bind( new IPEndPoint( ipAddress, port ) );
        socket.Listen( 10 );

        while ( !token.IsCancellationRequested )
        {
            using Socket connection = socket.Accept();
            Response response = onDataReceived( _serializer.Deserialize<Request>( connection ) );
            connection.Send( _serializer.Serialize( response ) );
        }
    }
}