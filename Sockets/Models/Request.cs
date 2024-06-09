using System.Text.Json;

namespace Sockets.Models;

public class Request
{
    public string RequestName { get; set; }
    public string JsonSerializedData { get; set; }

    public static Request Create<T>( string requestName, T content ) => new Request()
    {
        RequestName = requestName,
        JsonSerializedData = JsonSerializer.Serialize( content )
    };

    public static Request Create( string requestName, string content ) => new Request()
    {
        RequestName = requestName,
        JsonSerializedData = content
    };
}