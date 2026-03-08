namespace InvoiceSystem.Domain.ValueObjects;

public sealed class InvoiceApprovalPrediction
{
    public double PredictedHours { get; }
    public string ModelVersion { get; } = string.Empty;
    public DateTimeOffset EstimatedCompletionDate { get; }

    public InvoiceApprovalPrediction(double predictedHrs, string modelVersion,
        DateTimeOffset createdAt)
    {
        if(predictedHrs < 0)
            predictedHrs = Math.Max(0, predictedHrs);

        PredictedHours = predictedHrs;
        ModelVersion = modelVersion;
        EstimatedCompletionDate = createdAt.AddHours(predictedHrs);
    }

    public bool IsSLAAtRisk(TimeSpan slaThreshold)
        => TimeSpan.FromHours(PredictedHours) > slaThreshold;
}
