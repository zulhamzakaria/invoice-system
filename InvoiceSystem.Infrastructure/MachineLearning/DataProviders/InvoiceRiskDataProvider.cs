using InvoiceSystem.Application.Common.Models.ML;
using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using Microsoft.Extensions.Logging;

namespace InvoiceSystem.Infrastructure.MachineLearning.DataProviders;

public sealed class InvoiceRiskDataProvider : IRiskTrainingDataProvider
{
    private readonly AppDbContext _dbContext;
    ILogger<InvoiceRiskDataProvider> _logger;
    public InvoiceRiskDataProvider(AppDbContext dbContext,
        ILogger<InvoiceRiskDataProvider> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public Task<IEnumerable<InvoiceRiskTrainingRecord>> GetTrainingDataAsync(CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
