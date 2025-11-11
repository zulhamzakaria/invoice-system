using InvoiceSystem.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyUpdateDTO(
       [StringLength(50, MinimumLength = 1)] string? Name,
       [StringLength(10, MinimumLength = 1)] string? RegistrationNumber,
       AddressUpdateDTO? ShippingAddress,
       AddressUpdateDTO BillingAddress
    );
