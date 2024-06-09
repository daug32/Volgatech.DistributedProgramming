using System.Net;
using System.Net.Sockets;
using Sockets.Implementation;
using Sockets.Models;

namespace Sockets.Listeners;

public class Listener
{
    private static readonly Serializer _serializer = new();

    public readonly IPAddress Host;
    public readonly int Port;

    public Listener( string host, int port )
    {
        Host = new IpAddressCreator().Create( host );
        Port = port;
    }

    public void Listen( Func<Request?, Response> onDataReceived )
    {
        Listen( onDataReceived, CancellationToken.None );
    }

    public void Listen(
        Func<Request?, Response> onDataReceived,
        CancellationToken token )
    {
        using var socket = new Socket( Host.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
        socket.Bind( new IPEndPoint( Host, Port ) );
        socket.Listen( 10 );

        while ( !token.IsCancellationRequested )
        {
            using Socket connection = socket.Accept();
            Response response = onDataReceived( _serializer.Deserialize<Request>( connection ) );
            connection.Send( _serializer.Serialize( response ) );
        }
    }
}