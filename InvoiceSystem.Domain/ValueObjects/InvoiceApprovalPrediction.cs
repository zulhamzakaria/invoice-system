namespace InvoiceSystem.Domain.ValueObjects;

public sealed class InvoiceApprovalPrediction
{
    public double PredictedHours { get; }
    public string ModelVersion { get; } = string.Empty;
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset EstimatedCompletionDate { get; }
    public InvoiceApprovalPrediction(double predictedHrs, string modelVersion)
    {
        if(predictedHrs < 0)
            predictedHrs = Math.Max(0, predictedHrs);

        PredictedHours = predictedHrs;
        ModelVersion = modelVersion;
        CreatedAt = DateTimeOffset.UtcNow;
        EstimatedCompletionDate = CreatedAt.AddHours(predictedHrs);
    }

    public bool IsSLAAtRisk(TimeSpan slaThreshold)
        => TimeSpan.FromHours(PredictedHours) > slaThreshold;
}
