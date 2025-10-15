using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Mappers;

public class CompanyMapper
{
    public static CompanyDetailsDTO ToDetailsDTO(Company company)
    {

        var billingAddress = company.Addresses.Single(c => c.Type == AddressType.Billing);
        var billingAdressDTO = AddressMapper.ToAddressDTO(billingAddress);
        var shippingAddress = company.Addresses.Single(c => c.Type == AddressType.Shipping);
        var shippingAdressDTO = AddressMapper.ToAddressDTO(shippingAddress);


        return new CompanyDetailsDTO(
            company.Id,
            company.Name,
            company.RegistrationNumber,
            billingAdressDTO,
            shippingAdressDTO
            );
    }
}
