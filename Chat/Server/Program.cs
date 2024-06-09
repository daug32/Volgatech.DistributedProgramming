using System;
using Sockets.Listeners;

namespace Server;

internal static class Program
{
    private static void Main( string[] args )
    {
        if ( !Int32.TryParse( "7000", out int port ) )
        {
            Console.WriteLine( "Invalid port number. Use <port>" );
            return;
        }

        var application = new Application( new Listener( "localhost", port ) );

        application.Run();
    }
}