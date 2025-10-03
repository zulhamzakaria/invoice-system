using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class Company : Entity
{

    private const int MinLength = 1;
    private const int MaxCompanyNameLength = 50;
    private const int MaxRegistrationNoLength = 10;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string RegistrationNumber { get; private set; }
    public List<Address> Addresses { get; private set; } = new();

    private Company() { } // For EF Core

    public Company(string name, string registrationNumber)
    {
        Name = name.Trim();
        RegistrationNumber = registrationNumber.Trim();
    }

    public static Result<Company> Create(string name, string registrationNumber)
    {
        var errors = new List<Error>();
        string trimmedName = name.Trim();
        string trimmedRegistrationNumber = registrationNumber.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingName,"Company Name is required"));
        if (string.IsNullOrWhiteSpace(trimmedRegistrationNumber))
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingRegNo, "Company Registration No is required"));
        if (errors.Any())
            return Result<Company>.Failure(errors);
        var company = new Company(name, registrationNumber);
        return Result<Company>.Success(company);
    }

    public void AddAddress(Address address)
    {
        Addresses.Add(address);
    }
}
