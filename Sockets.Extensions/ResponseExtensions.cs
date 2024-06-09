﻿using System.Text.Json;
using Sockets.Models;

namespace Sockets.Extensions;

public static class ResponseExtensions
{
    public static Response ThrowIfNull( this Response? response )
    {
        return response ?? throw new ArgumentException( "Response was not provided" );
    }

    public static Response ThrowIfError( this Response response )
    {
        if ( response.IsSuccess )
        {
            return response;
        }

        string message = String.IsNullOrEmpty( response.Data )
            ? ""
            : $"Message: {response.Data}";

        throw new ArgumentException( $"Error. {message}" );
    }

    public static T? Parse<T>( this Response response ) where T : class
    {
        return String.IsNullOrEmpty( response.Data )
            ? null
            : JsonSerializer.Deserialize<T>( response.Data! );
    }
}