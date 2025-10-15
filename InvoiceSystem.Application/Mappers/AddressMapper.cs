using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public class AddressMapper
{
    public static AddressDTO ToAddressDTO(Address address)
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
