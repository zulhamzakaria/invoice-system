namespace InvoiceSystem.Domain.Interfaces;

public interface IApprover
{
    bool canApprove(decimal amount);
    bool isLimitlessApprover { get; }
    decimal maxApprovalAmount { get; }
}
