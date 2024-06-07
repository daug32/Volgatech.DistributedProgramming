using Microsoft.Extensions.Configuration;

namespace Infrastructure.Common;

// ReSharper disable once InconsistentNaming
public static class IConfigurationExtensions
{
    public static IConfigurationBuilder AddCommonConfiguration( this IConfigurationBuilder builder )
    {
        return builder
            .AddJsonFile( "appsettings.json", optional: false )
            .AddJsonFile( $"appsettings.{EnvironmentHelper.GetDotnetEnvironment()}.json", optional: true );
    }
}