namespace InvoiceSystem.Application.Common.Models.ML;

public interface IMLTrainingDataProvidercs
{
    Task<IEnumerable<InvoiceRiskTrainingRecord>> GetTrainingDataAsync(CancellationToken token = default);
}
