using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public class FO : Employee, IApprover
{
    public decimal ApprovalLimit { get; private set; }

    public static Result<FO> Create(string name, string email, decimal approvalLimit)
    {
        var baseResult = CreateBase(name, email);
        var errors = baseResult.Errors.ToList();

        if (approvalLimit < 0)
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.MissingFOApprovalLimit, "Approval Limit is required"));
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
        ApprovalLimit = approvalLimit;
    }

    public bool canApprove(decimal amount) => amount <= ApprovalLimit;
}