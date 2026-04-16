using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceSystem.MLTrainer;

public class Runner
    (ILogger<Runner> logger, ICSVExporter csvExporter, 
    IRiskTrainingDataProvider riskTrainingDataProvider,
    IOptions<MLSettings> mlSettings,
    IRiskModelTrainer riskModelTrainer)
{
    public Task<int> RunAsync(CancellationToken ct)
    {

        var dataPath = mlSettings.Value.CsvFilePath;
        var modelPath = mlSettings.Value.ModelFilePath;


        throw new NotImplementedException();
    }
}
