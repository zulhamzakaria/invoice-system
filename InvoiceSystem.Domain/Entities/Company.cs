using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using static InvoiceSystem.Domain.Entities.Address;

namespace InvoiceSystem.Domain.Entities;

public class Company : Entity
{

    private const int MinLength = 1;
    private const int MaxCompanyNameLength = 50;
    private const int MaxRegistrationNoLength = 10;

    public string Name { get; private set; }
    public string RegistrationNumber { get; private set; }
    public IReadOnlyList<Address> Addresses { get; private set; }

    private Company() { } // For EF Core

    private Company(string name, string registrationNumber, IReadOnlyList<Address> addresses) : base()
    {
        Name = name;
        RegistrationNumber = registrationNumber;
        Addresses = addresses;
    }

    public static Result<Company> Create(string name, string registrationNumber, IReadOnlyList<Address> addresses)
    {
        var errors = new List<Error>();
        string trimmedName = name.Trim();
        string trimmedRegistrationNumber = registrationNumber.Trim();

        if (string.IsNullOrWhiteSpace(trimmedName))
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingName, "Company Name is required"));
        if (string.IsNullOrWhiteSpace(trimmedRegistrationNumber))
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingRegNo, "Company Registration No is required"));
        if (!string.IsNullOrWhiteSpace(trimmedName) && trimmedName.Length > MaxCompanyNameLength)
            errors.Add(Error.Validation(CompanyErrors.Creation.NameLengthViolation, $"Name length must be between {MinLength} and {MaxCompanyNameLength}"));
        if (!string.IsNullOrWhiteSpace(registrationNumber) && registrationNumber.Length > MaxRegistrationNoLength)
            errors.Add(Error.Validation(CompanyErrors.Creation.RegistrationNoViolation, $"Registration No length must be between {MinLength} and {MaxRegistrationNoLength}"));
        if (addresses == null || !addresses.Any())
        {
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingAddresses, "No Addresses (Billing nor Shipping) provided."));
        }
        else
        {
            if (!addresses.Any(a => a.Type == AddressType.Billing))
            {
                errors.Add(Error.Validation(CompanyErrors.Creation.MissingBillingAddress, "A Billing Address is required."));
            }
            if (!addresses.Any(a => a.Type == AddressType.Shipping))
            {
                errors.Add(Error.Validation(CompanyErrors.Creation.MissingShippingAddress, "A Shipping Address is required."));
            }
        }
        if (errors.Any())
            return Result<Company>.Failure(errors);
        var company = new Company(trimmedName, trimmedRegistrationNumber, addresses);
        return Result<Company>.Success(company);
    }
}
