using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Domain.Entities;

public abstract class Employee : AuditableEntity
{

    private const int MinLength = 1;
    private const int MaxNameLength = 100;
    private const int MaxEmailLength = 100;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string EmployeeCode { get; private set; } = GeneratedCode();

    private static string GeneratedCode()
    {
        var random = Guid.NewGuid().ToString("N")[..6].ToUpper();
        return $"EMP-{DateTime.UtcNow:yyyyMMdd}-{random}";
    }

    public string Name { get; private set; }
    public string Email { get; private set; }

    public EmployeeStatus Status { get; private set; } = EmployeeStatus.Active;
    protected Employee()
    {
        //Status = EmployeeStatus.Active;
    } // For EF Core



    protected static Result<Employee> CreateBase(string name, string email)
    {
        var errors = new List<Error>();
        var trimmedName = name.Trim();
        var trimmedEmail = email.Trim();

        if (string.IsNullOrEmpty(trimmedName))
            errors.Add(Error.Validation(EmployeeErrors.Creation.MissingName, "Name is required"));
        if (string.IsNullOrEmpty(trimmedEmail))
            errors.Add(Error.Validation(EmployeeErrors.Creation.MissingEmail, "Email is required"));
        if (!string.IsNullOrEmpty(trimmedName) && trimmedName.Length > MaxNameLength)
            errors.Add(Error.Validation(EmployeeErrors.Creation.NameLengthViolation, $"Name length must be between {MinLength} and {MaxNameLength} characters"));
        if (!string.IsNullOrEmpty(trimmedEmail) && trimmedEmail.Length > MaxEmailLength)
            errors.Add(Error.Validation(EmployeeErrors.Creation.EmailLengthViolation, $"Email length must be between {MinLength} and {MaxEmailLength} characters"));

        if (errors.Count > 0)
            return Result<Employee>.Failure(errors);
        return Result<Employee>.Success(null);
    }

    protected Employee(string name, string email)
    {
        Name = name;
        Email = email;
        Status = EmployeeStatus.Active;
    }

    public void Deactivate() =>
            Status = EmployeeStatus.Inactive;

    public void Activate() =>
            Status = EmployeeStatus.Active;

    public Result<Employee> UpdateName(string name)
    {
        var errors = new List<Error>();
        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.MissingName, "Name is required"));
        }
        var trimmedName = name.Trim();
        if (!string.IsNullOrEmpty(trimmedName) && trimmedName.Length > MaxNameLength)
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.NameLengthViolation, $"Name length must be between {MinLength} and {MaxNameLength} characters"));
        }

        if(errors.Count > 0) return Result<Employee>.Failure(errors);

        Name = trimmedName;
        return Result<Employee>.Success(this);
    }

    public Result<Employee> UpdateEmail(string email)
    {
        var errors = new List<Error>();
        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.MissingEmail, "Email is required"));
        }
        var trimmedEmail = email.Trim();
        if (!string.IsNullOrEmpty(trimmedEmail) && trimmedEmail.Length > MaxEmailLength)
        {
            errors.Add(Error.Validation(EmployeeErrors.Creation.EmailLengthViolation, $"Email length must be between {MinLength} and {MaxEmailLength} characters"));
        }

        if (errors.Count > 0) return Result<Employee>.Failure(errors);

        Email = trimmedEmail;
        return Result<Employee>.Success(this);
    }

    public Result<Employee> PatchEmployee(string? name, string? email, decimal? maxApprovalLimit)
    {
        if(maxApprovalLimit is not null) UpdateMaxApprovalLimit((decimal)maxApprovalLimit);
    }

    public Result<Employee> UpdateName(string name)
    {
        var trimmedName = name.Trim();
        if()
    }

    private Result<Employee> UpdateMaxApprovalLimit(decimal maxApprovalLimit)
    {
        if(this is not IApprover approver)
        {
            return Result<Employee>.Failure(Error.Validation(EmployeeErrors.Updating.InvalidApprover, "The Employee is not an Approver"));
        }
        if(maxApprovalLimit < 0)
        {
            return Result<Employee>.Failure(Error.Validation(EmployeeErrors.Updating.InvalidApprovalAmount, "Approval Limit cannot be lesser than zero"));
        }
        //update approval limit
        approver.UpdateApprovalLimit(maxApprovalLimit);
        return Result<Employee>.Success(this);
    }
}
