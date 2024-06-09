using System;
using Sockets.Listeners;

namespace Server;

internal static class Program
{
    private static void Main( string[] args )
    {
        string helpMessage = "Use <port>";
        if ( args.Length != 1 )
        {
            Console.WriteLine( $"Invalid number of arguments. {helpMessage}" );
            return;
        }
        
        if ( !Int32.TryParse( args[0], out int port ) )
        {
            Console.WriteLine( $"Invalid port number. Use {helpMessage}" );
            return;
        }

        var application = new Application( new Listener( "localhost", port ) );

        application.Run();
    }
}