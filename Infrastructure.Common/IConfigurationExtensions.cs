using Microsoft.Extensions.Configuration;

namespace Infrastructure.Common;

// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    public static IConfigurationBuilder AddCommonConfiguration( this IConfigurationBuilder builder )
    {
        string environment = Environment.GetEnvironmentVariable( "ASPNETCORE_ENVIRONMENT" ) ?? "";
        
        return builder
            .AddJsonFile( "appsettings.json", optional: false )
            .AddJsonFile( $"appsettings.{environment}.json", optional: true );
    }
}