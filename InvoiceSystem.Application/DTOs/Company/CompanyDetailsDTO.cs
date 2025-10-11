using InvoiceSystem.Application.DTOs.Address;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceSystem.Application.DTOs.Company;

public record CompanyDetailsDTO(Guid Id,
                                [property: JsonPropertyName("company_name")] string Name, 
                                string RegistrationNumber, 
                                AddressDTO ShippingAddress, 
                                AddressDTO BillingAddress);
