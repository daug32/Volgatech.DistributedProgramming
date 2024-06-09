using System;
using System.Threading;
using Sockets.Listeners;
using Sockets.Models;

namespace Server;

internal static class Program
{
    private static readonly Listener _listener = new();

    private static void Main( string[] args )
    {
        if ( !Int32.TryParse( "7000", out int port ) )
        {
            Console.WriteLine( "Invalid port number. Use <port>" );
            return;
        }

        StartListening( "localhost", port );
    }

    private static void StartListening( string host, int port )
    {
        Console.WriteLine( $"Server started listing on \"{host}:{port}\"..." );

        _listener.Listen(
            host,
            port,
            OnDataReceived,
            CancellationToken.None );

        Console.WriteLine( "Server stopped" );
    }

    private static void OnDataReceived( Request? request )
    {
        string message = request?.Data ?? "NULL";
        Console.WriteLine( $"Received message: {message}" );
    }
}