using MessageBus.Interfaces;
using Microsoft.Extensions.Logging;

namespace Valuator.RankCalculator;

public class Application : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IConsumersHandler _consumersHandler;
    private readonly ILogger _logger;
    
    public Application( IConsumersHandler consumersHandler, ILogger<Application> logger )
    {
        _consumersHandler = consumersHandler;
        _logger = logger;
    }

    protected override Task ExecuteAsync( CancellationToken token )
    {
        _logger.LogInformation( $"{typeof(Application).Assembly.GetName().Name} started" );
        
        _consumersHandler.Start();

        while ( !token.IsCancellationRequested )
        {
        }
        
        _consumersHandler.Stop();

        _logger.LogInformation( $"{typeof(Application).Assembly.GetName().Name} completed" );
        
        return Task.CompletedTask;
    }
}