using MessageBus.Interfaces;

namespace Valuator.BackgroundService;

public class Application : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IConsumersHandler _consumersHandler;
    
    public Application( IConsumersHandler consumersHandler )
    {
        _consumersHandler = consumersHandler;
    }

    protected override Task ExecuteAsync( CancellationToken token )
    {
        _consumersHandler.Start();

        while ( !token.IsCancellationRequested )
        {
        }
        
        _consumersHandler.Stop();
        
        return Task.CompletedTask;
    }
}