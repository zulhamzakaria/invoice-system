namespace InvoiceSystem.Domain.Enums;

public enum InvoiceStatus
{
    Draft,
    PendingOfficerApproval,
    PendingManagerApproval,
    //ApprovedByOfficer,
    ApprovedByManager,
    Rejected,
    Voided,
    Paid
}
