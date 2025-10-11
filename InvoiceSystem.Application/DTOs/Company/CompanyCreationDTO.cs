using InvoiceSystem.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyCreationDTO(
                [property: Required, StringLength(50, MinimumLength =1)]
                string Name,
                [property: Required, StringLength(10, MinimumLength =1)]
                string RegistrationNumber,
                [property: Required]
                AddressDTO ShippingAddress,
                [property: Required]
                AddressDTO BillingAddress);
