using System;
using System.Net.Sockets;
using System.Text.Json;
using Chatting.Commands.Commands;
using Chatting.Commands.Queries;
using Sockets.Connectors;
using Sockets.Extensions;
using Sockets.Models;

namespace Client;

public class Application( ConnectionCreator connectionCreator )
{
    public void Run( string message )
    {
        SendMessage( new SendMessageCommand { Message = message } );

        GetMessageHistoryQueryResult result = GetMessageHistory( new GetMessageHistoryQuery() );
        foreach ( string messageItem in result.MessagesHistory )
        {
            Console.WriteLine( messageItem );
        }
    }

    private GetMessageHistoryQueryResult GetMessageHistory( GetMessageHistoryQuery query )
    {
        Response response = Send( nameof( GetMessageHistoryQuery ), query );

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