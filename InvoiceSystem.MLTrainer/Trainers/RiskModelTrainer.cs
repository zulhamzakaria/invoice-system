using InvoiceSystem.Application.Common.Models.ML;
using Microsoft.ML;
using System.Text.Json;

namespace InvoiceSystem.MLTrainer.Trainers;

public sealed class RiskModelTrainer
{
    private readonly int _seeder;

    public RiskModelTrainer(int seed = 1)
    {
        _seeder = seed;
    }

    public void Train(string datasetPath)
    {
        var mlContext = new MLContext(_seeder);

        IDataView dataView = mlContext.Data.LoadFromTextFile<InvoiceRiskTrainingRecord>
            (
                datasetPath,
                hasHeader: true,
                separatorChar: ','
            );

        var pipeline = mlContext.Transforms.Concatenate(
            "Features", 
            nameof(InvoiceRiskTrainingRecord.Amount),
            nameof(InvoiceRiskTrainingRecord.VendorAverageAmount),
            nameof(InvoiceRiskTrainingRecord.IsNewVendor))
            .Append(mlContext.BinaryClassification.Trainers.FastTree(
                labelColumnName: "Label", 
                numberOfLeaves: 20,
                numberOfTrees: 100));

        var model = pipeline.Fit(dataView);
        string outputFolder = "Output";
        string modelPath = Path.Combine(outputFolder, "RiskModel.zip");
        string metadataPath = Path.Combine(outputFolder, "RiskModelMetadata.json");

        Directory.CreateDirectory(outputFolder);
        mlContext.Model.Save(model, dataView.Schema, modelPath);

        var metrics = mlContext.BinaryClassification.Evaluate(model.Transform(dataView));
        var metadata = new
        {
            TrainingDate = DateTime.UtcNow,
            Algorithm = "FastTree",
            Parameters = new { Seed = _seeder, Leaves = 20, Trees = 100},
            Performance = new
            {
                Accuracy = metrics.Accuracy,
                AreaUnderRocCurve = metrics.AreaUnderRocCurve,
            }
        };

        File.WriteAllText(metadataPath,
            JsonSerializer.Serialize(metadata, 
            new JsonSerializerOptions { WriteIndented = true }));

    }
}
