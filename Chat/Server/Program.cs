using System;
using System.Text.Json;
using System.Threading;
using Chatting.Dtos;
using Sockets.Listeners;
using Sockets.Models;

namespace Server;

internal static class Program
{
    private static readonly Listener _listener = new();

    private static void Main( string[] args )
    {
        if ( !Int32.TryParse( "7000", out int port ) )
        {
            Console.WriteLine( "Invalid port number. Use <port>" );
            return;
        }

        StartListening( "localhost", port );
    }

    private static void StartListening( string host, int port )
    {
        Console.WriteLine( $"Server started listing on \"{host}:{port}\"..." );

        _listener.Listen(
            host,
            port,
            OnDataReceived,
            CancellationToken.None );

        Console.WriteLine( "Server stopped" );
    }

    private static Response OnDataReceived( Request? request )
    {
        string? message = request?.Data;
        if ( message is null )
        {
            Console.WriteLine( "Message is null" );
            return Response.Failed( "Unknown format" );
        }

        if ( request?.RequestName == nameof( SendMessageCommand ) )
        {
            var command = JsonSerializer.Deserialize<SendMessageCommand>( request.Data );
            Console.WriteLine( $"Received message. Message: \"{command?.Message ?? "NULL"}\"" );
            return Response.Ok();
        }
        
        return Response.Failed( "Unknown request" );
    }
}