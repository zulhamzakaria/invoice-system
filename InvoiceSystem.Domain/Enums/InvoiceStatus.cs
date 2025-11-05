namespace InvoiceSystem.Domain.Enums;

public enum InvoiceStatus
{
    Draft,
    Submitted,
    PendingApproval,
    Approved,
    Rejected,
    Voided,
    Paid
}
