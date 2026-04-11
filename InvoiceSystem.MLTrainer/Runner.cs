using Microsoft.Extensions.Logging;

namespace InvoiceSystem.MLTrainer;

public interface IRunner
{
    Task<int> RunAsync(CancellationToken ct);
}

public class Runner : IRunner
{
    private readonly ILogger<Runner> _logger;
    public Runner(ILogger<Runner> logger)
    {
        _logger = logger;
    }
    public Task<int> RunAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
