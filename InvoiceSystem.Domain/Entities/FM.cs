using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public class FM : Employee, IApprover
{
    public decimal ApprovalLimit { get; private set; }

    public bool IsLimitlessApprover => true;

    public decimal MaxApprovalAmount => decimal.MaxValue;

    public EmployeeType EmployeeType => EmployeeType.FM;

    private FM() { } // For EF Core

    /// <summary>FM can approve any amount by default, or a custom limit if specified.</summary>
    public FM(string name, string email, decimal approvalLimit = decimal.MaxValue) : base(name, email)
    {
        ApprovalLimit = approvalLimit;
    }

    public bool CanApprove(decimal amount) { 
        if(IsLimitlessApprover)
            return true;
        return amount <= MaxApprovalAmount; 
    }

    public void UpdateApprovalLimit(decimal maxApprovalAmount)
    {
        ApprovalLimit = maxApprovalAmount;
    }
}
