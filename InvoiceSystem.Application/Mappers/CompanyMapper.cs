using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Mappers;

public class CompanyMapper: ICompanyMapper
{
    private readonly IAddressMapper _addressMapper;
    public CompanyMapper(IAddressMapper addressMapper)
    {
        _addressMapper = addressMapper;
    }

    public List<CompanySummaryDTO> MapToSummaryDTOs(IReadOnlyList<Company> companies)
    {
        return companies.Select(c => new CompanySummaryDTO(c.Id, c.Name, c.RegistrationNumber)).ToList();
    }

    public CompanyDetailsDTO ToDetailsDTO(Company company)
    {

        var billingAddress = company.Addresses.Single(c => c.Type == AddressType.Billing);
        var billingAdressDTO = _addressMapper.ToAddressDTO(billingAddress);
        var shippingAddress = company.Addresses.Single(c => c.Type == AddressType.Shipping);
        var shippingAdressDTO = _addressMapper.ToAddressDTO(shippingAddress);


        return new CompanyDetailsDTO(
            company.Id,
            company.Name,
            company.RegistrationNumber,
            billingAdressDTO,
            shippingAdressDTO
            );
    }
}
