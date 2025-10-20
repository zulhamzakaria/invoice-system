using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

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
            errors.Add(Error.Validation(EmployeeErrors.Creation.NameLengthViolation, $"Street length must be between {MinLength} and {MaxNameLength} characters"));
        if (!string.IsNullOrEmpty(trimmedEmail) && trimmedEmail.Length > MaxEmailLength)
            errors.Add(Error.Validation(EmployeeErrors.Creation.EmailLengthViolation, $"Street length must be between {MinLength} and {MaxEmailLength} characters"));

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

}
