using System.Net.Sockets;
using Sockets.Connectors;
using Sockets.Models;

namespace Sockets.Extensions;

public static class SocketExtensions
{
    public static Response? SendWithResponse( this Socket socket, Request request )
    {
        socket.Send( request );
        return socket.Receive();
    }
}