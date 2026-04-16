namespace InvoiceSystem.MLTrainer;

public sealed class MLSettings
{
    public string CsvFilePath { get; init; } = "Data/training-set.csv";
    public string ModelFilePath { get; init; } = "Models/model.zip";
}
