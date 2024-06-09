namespace Sockets.Models;

public class Request
{
    public string RequestName { get; set; }
    public string Data { get; set; }

    public static Request Create( string requestName, string content )
    {
        return new Request
        {
            RequestName = requestName,
            Data = content
        };
    }
}