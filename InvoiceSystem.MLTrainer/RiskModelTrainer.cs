namespace InvoiceSystem.MLTrainer;

public interface IRiskModelTrainer
{
    Task TrainModelAsync(CancellationToken ct);
}
internal class RiskModelTrainer
{
}
