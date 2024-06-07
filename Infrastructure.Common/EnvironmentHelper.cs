namespace Infrastructure.Common;

public static class EnvironmentHelper
{
    public static string GetDotnetEnvironment() => 
        Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" )
        ?? Environment.GetEnvironmentVariable( "DOTNET_ENVIRONMENT" ) 
        ?? String.Empty;
    
    public static Dictionary<string, string> GetRedisConnections( IEnumerable<string> regions ) => regions
        .Select( region => ( region: region, environment: Environment.GetEnvironmentVariable( $"DB_{region.ToUpper()}" ) ) )
        .Where( pair => !String.IsNullOrWhiteSpace( pair.environment ) )
        .ToDictionary( 
            pair => pair.region,
            pair => pair.environment! );
}