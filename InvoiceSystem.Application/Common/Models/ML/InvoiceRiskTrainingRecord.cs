using Microsoft.ML.Data;

namespace InvoiceSystem.Application.Common.Models.ML;

public sealed class InvoiceRiskTrainingRecord
{
    [LoadColumn(0)]
    public float Amount { get; set; }
    [LoadColumn(1)]
    public float VendorAverageAmount { get; set; }
    [LoadColumn(2)]
    public bool IsNewVendor { get; set; }
    [LoadColumn(3)]
    [ColumnName("Label")]
    public bool IsHighRisk { get; set; }
}
