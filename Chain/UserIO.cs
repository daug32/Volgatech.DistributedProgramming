namespace Chain;

public class UserIo
{
    public static int AskUserForValue()
    {
        while ( true )
        {
            Console.Write( "Enter value: " );

            string? rawValue = Console.ReadLine();
            if ( String.IsNullOrWhiteSpace( rawValue ) )
            {
                Console.WriteLine( "No value provided. Try again" );
                continue;
            }

            if ( !Int32.TryParse( rawValue, out int result ) )
            {
                Console.WriteLine( "This is not a number. Try again" );
                continue;
            }

            return result;
        }
    }
}