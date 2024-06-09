using System.Text.Json;

namespace Sockets.Models;

public class Request
{
    public string Data { get; set; }

    public static Request Create<T>( T content ) => new Request()
    {
        Data = JsonSerializer.Serialize( content )
    };
    
    public static Request Create( string content ) => new Request()
    {
        Data = content
    };
}