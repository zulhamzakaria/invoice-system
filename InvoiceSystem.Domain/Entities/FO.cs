using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public class FO : Employee, IApprover
{
    public decimal ApprovalLimit { get; private set; }

    private FO() { } // For EF Core
    public FO(string name, string email, decimal approvalLimit) : base(name, email)
    {
        ApprovalLimit = approvalLimit;
    }

    public bool canApprove(decimal amount) => amount <= ApprovalLimit;
}