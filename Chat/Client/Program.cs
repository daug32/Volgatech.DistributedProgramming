using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using Chatting.Dtos;
using Sockets.Connectors;
using Sockets.Extensions;
using Sockets.Models;

namespace Client;

internal static class Program
{
    private static readonly ConnectionCreator _connectionCreator = new();
    
    public static void Main( string[] args )
    {
        if ( !Int32.TryParse( "7000", out int port ) )
        {
            Console.WriteLine( "Invalid port number. Use <host> <port> <message>" );
            return;
        }
        
        using Socket connection = _connectionCreator.ConnectToServer( "localhost", port );
        
        Response response = connection
            .SendWithResponse( Request.Create(
                requestName: nameof( SendMessageCommand ),
                content: JsonSerializer.Serialize( new SendMessageCommand 
                {
                    Message = "Some message"
                } ) ) )
            .ThrowIfNull()
            .ThrowIfError();

        string message = String.IsNullOrEmpty( response.Data )
            ? ""
            : $"Message: {response.Data}";
        Console.WriteLine( $"Successfully sent message. {message}" );

        connection.Disconnect( false );
    }
}