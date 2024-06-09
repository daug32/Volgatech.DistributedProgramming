namespace WaveAlgorithm;

internal static class Program
{
    public static void Main( string[] args )
    {
        var help = "Use <currentPort> <nextHost> <nextPort> [<\"true\" if this process is initiator>]";

        if ( args.Length < 3 || args.Length > 4 )
        {
            Console.WriteLine( "Invalid number arguments" );
            return;
        }
        
        string nextHost = args[1];
        
        if ( !Int32.TryParse( args[2], out int nextPort ) ||
             !Int32.TryParse( args[0], out int currentPort ) )
        {
            Console.WriteLine( $"Invalid port. {help}" );
            return;
        }

        bool isInitiator = false;
        if ( args.Length > 3 && !Boolean.TryParse( args[3], out isInitiator ) )
        {
            Console.WriteLine( $"Invalid isInitiator value. {help}" );
            return;
        }

        var application = new Application(
            "localhost",
            currentPort,
            nextHost,
            nextPort,
            isInitiator );

        application.Run();
    }
}