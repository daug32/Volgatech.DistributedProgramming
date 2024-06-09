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
        result.AddRange( BitConverter.GetBytes( serializedData.Length ) );
        result.AddRange( Encoding.UTF8.GetBytes( serializedData ) );

        return result.ToArray();
    }

    public Request? Deserialize( Socket receiver )
    {
        var buffer = new byte[_maxBufferSize];

        ushort contentLength = ParsePackageLength( receiver );
        var result = new StringBuilder( contentLength );
        
        while ( contentLength > result.Length )
        {
            int writtenBytesLength = receiver.Receive( buffer );
            result.Append( Encoding.UTF8.GetString( buffer, 2, writtenBytesLength - 2 ) );
        }

        return JsonSerializer.Deserialize<Request>( result.ToString() );   
    }

    private ushort ParsePackageLength( Socket receiver )
    {
        var intBuffer = new byte[2];
        receiver.Receive( intBuffer );
        return BitConverter.ToUInt16( intBuffer, 0 );
    }
}