using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressCreationDTO(
    [Required]
    [StringLength(200, MinimumLength = 1)]
    string Street,
    [property: Required, StringLength(10, MinimumLength =1)]
    string Zipcode,
    [property: Required, StringLength(100, MinimumLength =1)]
    string City,
    [property: Required, StringLength(100,MinimumLength =1)]
    string State,
    [property: Required, StringLength(50, MinimumLength =1)]
    string Country,
    [property: Required] AddressType AddressType);
