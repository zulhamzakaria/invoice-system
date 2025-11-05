namespace InvoiceSystem.Domain.Enums;

public enum InvoiceStatus
{
    Draft,
    PendingApproval,
    PendingManagerApproval,
    Approved,
    Rejected,
    Voided,
    Paid
}
