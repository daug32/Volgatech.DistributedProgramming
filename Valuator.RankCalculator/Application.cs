using MessageBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace Valuator.RankCalculator;

public class Application( IConsumersHandler consumersHandler, ILogger<Application> logger ) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override Task ExecuteAsync( CancellationToken token )
    {
        logger.LogInformation( $"{typeof(Application).Assembly.GetName().Name} started" );
        
        consumersHandler.Start();

        while ( !token.IsCancellationRequested )
        {
        }
        
        consumersHandler.Stop();

        logger.LogInformation( $"{typeof(Application).Assembly.GetName().Name} completed" );
        
        return Task.CompletedTask;
    }
}