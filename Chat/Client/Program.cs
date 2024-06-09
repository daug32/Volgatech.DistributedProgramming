﻿using System;
using System.Net.Sockets;
using Chatting.Dtos;
using Sockets.Connectors;
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
        connection.Send( Request.Create(
            nameof( SendMessageCommand ),
            new SendMessageCommand
            {
                Message = "Some message"
            } ) );

        connection.Disconnect( false );
    }
}