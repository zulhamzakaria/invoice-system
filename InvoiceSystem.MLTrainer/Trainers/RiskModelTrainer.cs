using Microsoft.ML;

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
    }
}
