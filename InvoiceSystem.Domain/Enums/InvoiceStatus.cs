namespace InvoiceSystem.Domain.Enums;

public enum InvoiceStatus
{
    Draft,
    PendingOfficerApproval,
    PendingManagerApproval,
    Approved,
    Rejected,
    Voided,
    Paid
}
