using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class Company : Entity
{

    private const int MinLength = 1;
    private const int MaxCompanyNameLength = 50;
    private const int MaxRegistrationNoLength = 20;

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

    public Result UpdateAddresses(Address newBillingAddress, Address newShippingAddress)
    {       
        var preservedAddress = this.Addresses.Where(a => a.Type != AddressType.Billing && a.Type != AddressType.Shipping).ToList();
        preservedAddress.Add(newBillingAddress);
        preservedAddress.Add(newShippingAddress);
        this.Addresses = preservedAddress.AsReadOnly();
        return Result.Success();
    }

    public static Result<Company> Create(string name, string registrationNumber, Address billingAddress, Address shippingAddress)
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
        if (billingAddress is null)
        {
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingBillingAddress, "A Billing Address is required"));
        }
        if (shippingAddress is null)
        {
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingShippingAddress, "A Shipping Address is required"));
        }
        if (billingAddress != null && billingAddress.Type != AddressType.Billing)
        {
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingBillingAddress, "Billing address must be explicitly set to Billing."));
        }
        if (shippingAddress != null && shippingAddress.Type != AddressType.Shipping)
        {
            errors.Add(Error.Validation(CompanyErrors.Creation.MissingShippingAddress, "Shipping address must be explicitly set to Shipping."));
        }

        if (errors.Count > 0)
            return Result<Company>.Failure(errors);
        var addresses= new List<Address> { billingAddress, shippingAddress};
        var company = new Company(trimmedName, trimmedRegistrationNumber, addresses);
        return Result<Company>.Success(company);
    }
}
