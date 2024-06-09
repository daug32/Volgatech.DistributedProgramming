using System;
using Sockets.Connectors;

namespace Client;

internal static class Program
{
    public static void Main( string[] args )
    {
        if ( !Int32.TryParse( "7000", out int port ) )
        {
            Console.WriteLine( "Invalid port number. Use <host> <port> <message>" );
            return;
        }

        var application = new Application( new ConnectionCreator( "localhost", port ) );

        application.Run( "Hello, world!" );
    }
}