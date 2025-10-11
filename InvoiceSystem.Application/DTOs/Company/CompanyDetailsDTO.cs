using InvoiceSystem.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyDetailsDTO(
    Guid Id,
    [property: Required, StringLength(50, MinimumLength =1)]
    [property: JsonPropertyName("company_name")]
    string Name,
    [property: Required, StringLength(10, MinimumLength =1)]
    string RegistrationNumber,
    [property: Required]
    AddressDTO ShippingAddress,
    [property: Required]
    AddressDTO BillingAddress
    );
