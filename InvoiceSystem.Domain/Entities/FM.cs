namespace InvoiceSystem.Domain.Entities;

public class FM : Employee
{
    public decimal ApprovalLimit { get; private set; }

    private FM() { } // For EF Core
    public FM(string name, string email, decimal approvalLimit) : base(name, email)
    {
        ApprovalLimit = approvalLimit;
    }

    public bool CanApprove(decimal amount) => true; //limitless
}
