using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using Chatting.Dtos.Commands;
using Chatting.Dtos.Queries;
using Sockets.Listeners;
using Sockets.Models;

namespace Server;

public class Application( Listener listener )
{
    private readonly List<string> _messages = new();

    public void Run()
    {
        Console.WriteLine( $"Server started listening on \"{listener.Host}:{listener.Port}\"..." );

        listener.Listen(
            OnDataReceived,
            CancellationToken.None );

        Console.WriteLine( "Server stopped" );
    }
    
    private Response OnDataReceived( Request? request )
    {
        string? message = request?.Data;
        if ( message is null )
        {
            Console.WriteLine( "Message is null" );
            return Response.Failed( "Unknown format" );
        }

        return request?.RequestName switch
        {
            nameof( SendMessageCommand ) => HandleSendMessageCommand( request ),
            nameof( GetMessagesQuery ) => HandleGetMessagesQuery(),
            _ => Response.Failed( "Unknown request" )
        };
    }

    private Response HandleSendMessageCommand( Request request )
    {
        SendMessageCommand? sendMessageCommand = JsonSerializer.Deserialize<SendMessageCommand>( request.Data );
        
        string? message = sendMessageCommand?.Message;
        if ( message is null )
        {
            return Response.Failed( "Message is null" );
        }
        
        Console.WriteLine( $"Message received: {message}" );
        _messages.Add( message );
        
        return Response.Ok();
    }

    private Response HandleGetMessagesQuery()
    {
        var messages = new GetMessageHistoryQueryResult 
        {
            MessagesHistory = _messages.ToList() 
        };

        return Response.Ok( JsonSerializer.Serialize( messages ) );
    }
}