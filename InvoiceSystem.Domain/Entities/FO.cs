using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public class FO : Employee, IApprover
{
    public decimal MaxApprovalAmount { get; private set; }
    public bool IsLimitlessApprover => false;
    public static Result<FO> Create(string name, string email, decimal approvalLimit)
    {
        var baseResult = CreateBase(name, email);
        var errors = baseResult.Errors.ToList();

        if (approvalLimit < 0)
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.NegativeFOApprovalLimit, "Approval Limit cannot be in negative"));
        }

        if (errors.Any())
        {
            return Result<FO>.Failure(errors);
        }
        return Result<FO>.Success(new FO(name, email, approvalLimit));
    }

    private FO() { } // For EF Core
    public FO(string name, string email, decimal approvalLimit) : base(name, email)
    {
        MaxApprovalAmount = approvalLimit;
    }

    public bool CanApprove(decimal amount) => amount <= MaxApprovalAmount;

    public void UpdateApprovalLimit(decimal maxApprovalAmount)
    {
        MaxApprovalAmount = maxApprovalAmount;
    }
}