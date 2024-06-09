using System;
using Sockets.Connectors;

namespace Client;

internal static class Program
{
    public static void Main( string[] args )
    {
        string helpMessage = "Use <host> <port> <message>";
        if ( args.Length != 3 )
        {
            Console.WriteLine( $"Invalid number of arguments. {helpMessage}" );
            return;
        }
        
        if ( !Int32.TryParse( args[1], out int port ) )
        {
            Console.WriteLine( $"Invalid port number. {helpMessage}" );
            return;
        }

        string host = args[0];
        string message = args[2];

        var application = new Application( new ConnectionCreator( host, port ) );

        application.Run( message );
    }
}