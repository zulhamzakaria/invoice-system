using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public class FM : Employee, IApprover
{
    public decimal ApprovalLimit { get; private set; }

    private FM() { } // For EF Core

    /// <summary>FM can approve any amount by default, or a custom limit if specified.</summary>
    public FM(string name, string email, decimal approvalLimit = decimal.MaxValue) : base(name, email)
    {
        ApprovalLimit = approvalLimit;
    }

    public bool canApprove(decimal amount) => amount <= ApprovalLimit;
}
