using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public class AddressMapper : IAddressMapper
{
    public AddressDTO ToAddressDTO(Address address)
    {
        return new AddressDTO(
            address.Street,
            address.ZipCode,
            address.City,
            address.State,
            address.Country,
            address.Type.ToString()
            );
    }
}
