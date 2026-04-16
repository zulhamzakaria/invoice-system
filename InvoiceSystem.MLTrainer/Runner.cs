using Microsoft.Extensions.Logging;

namespace InvoiceSystem.MLTrainer;

public class Runner(ILogger<Runner> logger)
{
    public Task<int> RunAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
