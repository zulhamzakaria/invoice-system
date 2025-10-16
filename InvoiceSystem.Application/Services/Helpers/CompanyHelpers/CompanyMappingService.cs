using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Helpers.CompanyHelpers;

public class CompanyMappingService : ICompanyMappingService
{
    public Result<Address> MergeAndValidateAddress(Address currentAddress, AddressUpdateDTO? updateDto)
    {
        if (updateDto == null)
        {
            return Result<Address>.Success(currentAddress);
        }

        var mergedStreet = updateDto.Street ?? currentAddress.Street;
        var mergedZipcode = updateDto.Zipcode ?? currentAddress.ZipCode;
        var mergedCity = updateDto.City ?? currentAddress.City;
        var mergedState = updateDto.State ?? currentAddress.State;
        var mergedCountry = updateDto.Country ?? currentAddress.Country;

        var addressType = currentAddress.Type;

        return Address.Create(
            mergedStreet,
            mergedZipcode,
            mergedCity,
            mergedState,
            mergedCountry,
            addressType
        );
    }

    public Result<Address> ValidateAndCreateAddress(AddressCreationDTO dto, AddressType type)
    {
        return Address.Create(
            dto.Street!,
            dto.Zipcode!,
            dto.City!,
            dto.State!,
            dto.Country!,
            type
        );
    }

    public Result<(Address billingAddress, Address shippingAddress)> MergeAndValidateAddress(Company existingCompany, CompanyUpdateDTO companyUpdateDTO)
    {
        var allErrors = new List<Error>();
        var billingAddressDTO = companyUpdateDTO.BillingAddress;
        var shippingAddressDTO = companyUpdateDTO.ShippingAddress;

        var billingAddress = existingCompany.Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
        var mergedBillingAddress = MergeAndCreateAddress(billingAddress, companyUpdateDTO.BillingAddress, AddressType.Billing);
        if (mergedBillingAddress.IsFailure)
        {
            allErrors.AddRange(mergedBillingAddress.Errors);
        }

        var shippingAddress = existingCompany.Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);
        var mergedShippingAddress = MergeAndCreateAddress(shippingAddress, shippingAddressDTO, AddressType.Shipping);
        if (mergedShippingAddress.IsFailure)
        {
            allErrors.AddRange(mergedShippingAddress.Errors);
        }

        if (allErrors.Count > 0)
            return Result<(Address billingAddress, Address shippingAddress)>.Failure(allErrors);

        return Result<(Address billingAddress, Address shippingAddress)>.Success((mergedBillingAddress.Value!, mergedShippingAddress.Value!));
    }

    private Result<Address> MergeAndCreateAddress(Address? currentAddress, AddressUpdateDTO? updateDTO,  AddressType type)
    {
        string street = updateDTO?.Street ?? currentAddress?.Street ?? string.Empty;
        string zipcode = updateDTO?.Zipcode ?? currentAddress?.ZipCode ?? string.Empty;
        string city = updateDTO?.City ?? currentAddress?.City ?? string.Empty;
        string state = updateDTO?.State ?? currentAddress?.State ?? string.Empty;
        string country = updateDTO?.Country ?? currentAddress?.Country ?? string.Empty;


        return Address.Create(street, city, state, zipcode, country, type);
    }
}
