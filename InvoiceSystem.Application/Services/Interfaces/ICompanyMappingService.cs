using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface ICompanyMappingService
{
    Result<Address> ValidateAndCreateAddress(AddressCreationDTO dto, AddressType type);
    Result<Address> MergeAndValidateAddress(Address currentAddress, AddressUpdateDTO? updateDto);
}
