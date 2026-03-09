using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.ValueObjects;

public sealed class InvoiceRiskAssessment
{
    public double RiskScore { get; }
    public RiskAssessment RiskLevel { get; }
    public string ModelVersion { get; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; }

    public InvoiceRiskAssessment() { }
    public InvoiceRiskAssessment(double riskScore, string modelVersion)
    {
        if (riskScore is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(riskScore), "Risk score must be between 0 and 1.");

        RiskScore = riskScore;
        RiskLevel = CalculateRiskLevel(riskScore);
        ModelVersion = modelVersion;
        GeneratedAt = DateTimeOffset.UtcNow;
    }

    private RiskAssessment CalculateRiskLevel(double riskScore)
        => riskScore switch
        {
            >= 0.8 => RiskAssessment.High,
            >= 0.5 => RiskAssessment.Medium,
            _ => RiskAssessment.Low
        };
}

