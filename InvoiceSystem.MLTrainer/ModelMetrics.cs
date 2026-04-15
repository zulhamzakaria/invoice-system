namespace InvoiceSystem.MLTrainer;

public sealed record ModelMetric(
    double Accuracy,
    double F1Score,
    double AreaUnderRocCurve);

