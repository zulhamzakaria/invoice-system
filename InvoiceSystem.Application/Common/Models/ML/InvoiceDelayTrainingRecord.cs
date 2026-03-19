namespace InvoiceSystem.Application.Common.Models.ML;

public record InvoiceDelayTrainingRecord
{
    public float Amount { get; set; }
    public float VendorAverageAmount { get; set; }
    public bool IsNewVendor { get; set; }
    public int Month { get; set; }
    public int DayOfWeek { get; set; }
    public float FOWorkload { get; set; }  // Number of pending invoices
    public float ApprovalHours { get; set; }  // Numeric label (regression)
}
