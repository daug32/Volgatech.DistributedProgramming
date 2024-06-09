using System.Net.Sockets;
using Sockets.Implementation;
using Sockets.Models;

namespace Sockets.Connectors;

public static class SocketExtensions
{
    private static readonly Serializer _serializer = new();

    public static void Send( this Socket socket, Request request )
    {
        socket.Send( _serializer.Serialize( request ) );
    }

    public static Response? Receive( this Socket socket )
    {
        return _serializer.Deserialize<Response>( socket );
    }
}