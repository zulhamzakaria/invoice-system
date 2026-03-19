namespace InvoiceSystem.Application.Common.Models.ML.DataProviders;

public interface IRiskTrainingDataProvider
{
    Task<IEnumerable<InvoiceRiskTrainingRecord>> GetTrainingDataAsync(CancellationToken token = default);
}
