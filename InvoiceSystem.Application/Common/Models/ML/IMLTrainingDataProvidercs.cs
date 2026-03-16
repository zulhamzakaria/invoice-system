using InvoiceSystem.Domain.SharedContracts;

namespace InvoiceSystem.Application.Common.Models.ML;

public interface IMLTrainingDataProvidercs
{
    Task<IEnumerable<ITrainingData>> GetTrainingDataAsync(CancellationToken token = default);
}
