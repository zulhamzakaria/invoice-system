namespace InvoiceSystem.Domain.SharedContracts;

public interface ITrainingData
{
    float Amount { get; }
    float VendorAverageAmount { get; }
    bool IsNewVendor { get; }
    bool IsHighRisk { get; }
}
