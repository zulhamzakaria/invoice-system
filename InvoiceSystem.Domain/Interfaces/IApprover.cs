using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.Interfaces;

public interface IApprover
{
    bool CanApprove(decimal amount);
    bool IsLimitlessApprover { get; }
    EmployeeType EmployeeType { get; }
    decimal MaxApprovalAmount { get; }
    void UpdateApprovalLimit(decimal maxApprovalAmount);
}
