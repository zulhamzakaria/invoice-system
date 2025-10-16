using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Helpers.Company;

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

    public Result<(Address billingAddress, Address shippingAddress)> MergeAndValidateAddress(Company existingCompany, Compan )
    {

    }

    private Result<Address> MergeAndCreateAddress(Address currentAddress, AddressUpdateDTO? updateDTO,  AddressType type)
    {
        string street = updateDTO?.Street ?? currentAddress.Street;
        string zipcode = updateDTO?.Zipcode ?? currentAddress.ZipCode;
        string city = updateDTO?.City ?? currentAddress.City;
        string state = updateDTO?.State ?? currentAddress.State;
        string country = updateDTO?.Country ?? currentAddress.Country;


        return Address.Create(street, city, state, zipcode, country, type);
    }
}
