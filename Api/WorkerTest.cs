
namespace Api;

public class WorkerTest : BackgroundService
{
    private readonly ILogger<WorkerTest> _logger;

    public WorkerTest(ILogger<WorkerTest> logger)
    {
        this._logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            this._logger.LogInformation("Worker Test... ");

            await Task.Delay(1000);
        }
    }
}
