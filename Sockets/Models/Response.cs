﻿namespace Sockets.Models;

public class Response
{
    public bool IsSuccess { get; set; }
    public string? Data { get; set; }

    public static Response Ok( string? response = null ) => new Response()
    {
        IsSuccess = true,
        Data = response
    };

    public static Response Failed( string? response = null ) => new Response()
    {
        IsSuccess = false,
        Data = response
    };
}