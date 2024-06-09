using System;
using System.Net.Sockets;
using System.Text.Json;
using Chatting.Dtos.Commands;
using Chatting.Dtos.Queries;
using Sockets.Connectors;
using Sockets.Extensions;
using Sockets.Models;

namespace Client;

public class Application( ConnectionCreator connectionCreator )
{
    public void Run( string message )
    {
        SendMessage( new SendMessageCommand { Message = message } );

        GetMessageHistoryQueryResult result = GetMessageHistory( new GetMessagesQuery() );
        foreach ( string messageItem in result.MessagesHistory )
        {
            Console.WriteLine( messageItem );
        }
    }

    private GetMessageHistoryQueryResult GetMessageHistory( GetMessagesQuery query )
    {
        Response response = Send( nameof( GetMessagesQuery ), query );

        var result = response.Parse<GetMessageHistoryQueryResult>();
        if ( result is null )
        {
            throw new ArgumentException( "No response provided" );
        }

        return result;
    }

    private void SendMessage( SendMessageCommand command )
    {
        Send( nameof( SendMessageCommand ), command );
    }

    private Response Send<T>( string requestName, T content )
    {
        using Socket connection = connectionCreator.CreateConnection();

        connection.Send( Request.Create(
            requestName,
            JsonSerializer.Serialize( content ) ) );

        Response response = connection
            .Receive()
            .ThrowIfNull()
            .ThrowIfError();

        return response;
    }
}