using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Sockets.Models;

namespace Sockets.Implementation;

internal class Serializer
{
    private readonly int _maxBufferSize = 64;
    
    public byte[] Serialize( Request request )
    {
        string serializedData = JsonSerializer.Serialize( request );

        var result = new List<byte>();
        // Add length of the message
        result.AddRange( BitConverter.GetBytes( serializedData.Length ) );
        result.AddRange( Encoding.UTF8.GetBytes( serializedData ) );

        return result.ToArray();
    }

    public Request? Deserialize( Socket receiver )
    {
        var buffer = new byte[_maxBufferSize];

        int contentLength = ParsePackageLength( receiver );
        var result = new StringBuilder( contentLength );
        
        while ( contentLength > result.Length )
        {
            int writtenBytesLength = receiver.Receive( buffer );
            string bytes = Encoding.UTF8.GetString( buffer, 0, writtenBytesLength );
            result.Append( bytes );
        }

        return JsonSerializer.Deserialize<Request>( result.ToString() );   
    }

    private int ParsePackageLength( Socket receiver )
    {
        var intBuffer = new byte[sizeof( int )];
        receiver.Receive( intBuffer );
        return BitConverter.ToInt32( intBuffer, 0 );
    }
}