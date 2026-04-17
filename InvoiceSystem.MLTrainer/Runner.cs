using InvoiceSystem.Application.Common.Models.ML.DataProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvoiceSystem.MLTrainer;

public class Runner
    (ILogger<Runner> logger, 
    ICSVExporter csvExporter, 
    IOptions<MLSettings> mlSettings,
    IRiskModelTrainer riskModelTrainer)
{

    private readonly MLSettings _settings = mlSettings.Value;
    public async Task<int> RunAsync(CancellationToken ct)
    {
        try 
        {
            logger.LogInformation("ML Risk model training process...");

            //// Safer hardcoding
            //var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ML", "Data", "risk-training-data.csv");
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dataPath = Path.Combine(baseDirectory, _settings.CsvFilePath);
            var modelPath = Path.Combine(baseDirectory, _settings.ModelFilePath);

            var dataDir = Path.GetDirectoryName(dataPath);
            if (!string.IsNullOrEmpty(dataDir))
                Directory.CreateDirectory(dataDir);

            var modelDir = Path.GetDirectoryName(modelPath);
            if (!string.IsNullOrEmpty(modelDir))
                Directory.CreateDirectory(modelDir);

            logger.LogInformation("Streaming training data to {Path}...", dataPath);
            var count = await csvExporter.ExportToCsvAsync(dataPath, ct);

            if (count == 0)
            {
                logger.LogWarning("No training data was exported to CSV. Aborting model training.");
                return 1;
            }

            var metrics = await riskModelTrainer.TrainModelAsync(dataPath, modelPath, ct);
            logger.LogInformation("=== Training Complete ===");
            logger.LogInformation("Model saved to: {ModelPath}", modelPath);
            logger.LogInformation("Accuracy: {Acc:P2}", metrics.Accuracy);
            logger.LogInformation("F1 Score: {F1:F4}", metrics.F1Score);
            logger.LogInformation("AUC: {AUC:F4}", metrics.AreaUnderRocCurve);

            return 0;
        }
        catch(OperationCanceledException ex)
        {
            logger.LogWarning(ex, "ML training process was cancelled.");
            return 2;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "An error occurred during the ML training process.");
            return 1;
        }
    }
}
