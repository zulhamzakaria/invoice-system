using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace InvoiceSystem.MLTrainer;

public class Runner
    (ILogger<Runner> logger, ICSVExporter csvExporter, 
    IRiskTrainingDataProvider riskTrainingDataProvider,
    IOptions<MLSettings> mlSettings,
    IRiskModelTrainer riskModelTrainer)
{

    private readonly MLSettings _settings = mlSettings.Value;
    public Task<int> RunAsync(CancellationToken ct)
    {
         
        //// Safer hardcoding
        //var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ML", "Data", "risk-training-data.csv");
        var dataPath = _settings.CsvFilePath;
        var modelPath = _settings.ModelFilePath;


        throw new NotImplementedException();
    }
}
