using InvoiceSystem.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyCreationDTO(
                [Required, StringLength(50, MinimumLength =1)] string Name,
                [Required, StringLength(10, MinimumLength =1)] string RegistrationNumber,
                [Required] AddressCreationDTO ShippingAddress,
                [Required] AddressCreationDTO BillingAddress);
