using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.Common;

public static class InvoiceStatusRules
{
    public static readonly HashSet<InvoiceStatus> CanSubmit = 
        new() {InvoiceStatus.Draft, InvoiceStatus.Rejected };

    public static readonly HashSet<InvoiceStatus> CanVoid =
        new() { InvoiceStatus.Draft, InvoiceStatus.Rejected };

    public static readonly HashSet<InvoiceStatus> CanReject = 
        new() {InvoiceStatus.PendingOfficerApproval, InvoiceStatus.PendingManagerApproval };

    public static readonly HashSet<InvoiceStatus> CanApprove =
        new() { InvoiceStatus.PendingOfficerApproval, InvoiceStatus.PendingManagerApproval };
}
