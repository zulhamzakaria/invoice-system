using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers.Interfaces;

public interface IAddressMapper
{
    AddressDTO ToAddressDTO(Address address);
}
