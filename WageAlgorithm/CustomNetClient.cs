using System.Net.Sockets;
using System.Text.Json;
using Sockets.Connectors;
using Sockets.Extensions;
using Sockets.Models;

namespace WaveAlgorithm;

public class CustomNetClient( string host, int port )
{
    public void SafelySend<T>( string requestName, T content )
    {
        const int delayInMilliseconds = 1000;
        const int maxTryCount = 3;
        int currentTry = 0;

        while ( currentTry < maxTryCount )
        {
            try
            {
                Response response = Send( requestName, content );
                if ( response.IsSuccess )
                {
                    return;
                }
            }
            catch ( Exception exception )
            {
                Console.WriteLine( exception );
                currentTry++;
                Task.Delay( delayInMilliseconds ).Wait();
            }
        }
    }

    public Response Send<T>( string requestName, T content )
    {
        using Socket connection = new ConnectionCreator( host, port ).CreateConnection();

        connection.Send( Request.Create(
            requestName: requestName,
            JsonSerializer.Serialize( content ) ) );

        return connection
            .Receive()
            .ThrowIfNull()
            .ThrowIfError();
    }
}