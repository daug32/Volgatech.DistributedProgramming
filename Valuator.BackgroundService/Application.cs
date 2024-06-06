using Valuator.BackgroundService.Consumers;

namespace Valuator.BackgroundService;

public class Application : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly CalculateRankMessageConsumer _calculateRankMessageConsumer;

    public Application( CalculateRankMessageConsumer calculateRankMessageConsumer )
    {
        _calculateRankMessageConsumer = calculateRankMessageConsumer;
    }

    protected override Task ExecuteAsync( CancellationToken token )
    {
        _calculateRankMessageConsumer.Start();

        while ( !token.IsCancellationRequested )
        {
        }
        
        _calculateRankMessageConsumer.Stop();

        return Task.CompletedTask;
    }
}