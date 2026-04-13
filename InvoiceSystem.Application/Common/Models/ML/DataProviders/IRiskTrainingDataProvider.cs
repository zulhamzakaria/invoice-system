namespace InvoiceSystem.Application.Common.Models.ML.DataProviders;

public interface IRiskTrainingDataProvider
{
    IAsyncEnumerable<InvoiceRiskTrainingRecord> GetTrainingDataStream(CancellationToken token = default);
}
