using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyUpdateDTO(
       [StringLength(50, MinimumLength = 1), NotEqual("string")] string? Name,
       [StringLength(10, MinimumLength = 1), NotEqual("string")] string? RegistrationNumber,
       AddressUpdateDTO? ShippingAddress,
       AddressUpdateDTO BillingAddress
    );
