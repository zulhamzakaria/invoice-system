using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Application.Services.Helpers.EmployeeHelpers;

public static class EmployeeFactory
{
    public static Result<Employee> Create(string name, string email, EmployeeType type, decimal? maxApprovalLimit)
    {
        return type switch
        {
            EmployeeType.Clerk => Result<Employee>.Success(new Clerk(name, email)),

            EmployeeType.FO => maxApprovalLimit is null || maxApprovalLimit <= 0 ?
            Result<Employee>.Failure(Error.Validation("", "Finance Officer must have a positive max approval limit")) :
            Result<Employee>.Success(new FO(name, email, (decimal)maxApprovalLimit)),

            EmployeeType.FM => Result<Employee>.Success(new FM(name, email, decimal.MaxValue)),
            
            _ => Result<Employee>.Failure(Error.Validation("","Invalid Employee type"))

        };
    }
}
