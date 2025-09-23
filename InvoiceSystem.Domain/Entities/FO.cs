namespace InvoiceSystem.Domain.Entities;

public class FO : Employee
{
    public decimal ApprovalLimit { get; private set; }

    private FO() { } // For EF Core
    public FO(string name, string email, decimal approvalLimit) : base(name, email)
    {
        ApprovalLimit = approvalLimit;
    }

    public bool CanApprove(decimal amount) => amount <= ApprovalLimit;
}