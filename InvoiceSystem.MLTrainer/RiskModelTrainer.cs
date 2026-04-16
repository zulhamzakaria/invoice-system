using InvoiceSystem.Application.Common.Models.ML;
using Microsoft.Extensions.Logging;
using Microsoft.ML;

namespace InvoiceSystem.MLTrainer;

public interface IRiskModelTrainer
{
    Task<ModelMetric> TrainModelAsync
        (string dataPath, string modelPath, CancellationToken ct);
}
public sealed class RiskModelTrainer(ILogger<RiskModelTrainer> logger) : IRiskModelTrainer
{
    public async Task<ModelMetric> TrainModelAsync(string dataPath, string modelPath, CancellationToken ct)
    {
       var mlContext  = new MLContext(seed:42);
        var trainingDate = mlContext.Data.LoadFromTextFile<InvoiceRiskTrainingRecord>
            (dataPath, hasHeader: true, separatorChar: ',');
        var pipeline = mlContext.Transforms.CopyColumns("Label", nameof(InvoiceRiskTrainingRecord.IsHighRisk))
            .Append(mlContext.Transforms.Concatenate("Features", 
            nameof(InvoiceRiskTrainingRecord.Amount),
            nameof(InvoiceRiskTrainingRecord.VendorAverageAmount)))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        logger.LogInformation("Training and evaluating the model ...");
        var model = await Task.Run(() => pipeline.Fit(trainingDate), ct);
        mlContext.Model.Save(model, trainingDate.Schema, modelPath);

        var metrics = mlContext.BinaryClassification
            .Evaluate(model.Transform(trainingDate));
        return new ModelMetric
            (metrics.Accuracy, metrics.F1Score, metrics.AreaUnderRocCurve);
    }
}
