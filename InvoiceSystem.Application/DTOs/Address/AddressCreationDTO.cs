using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressCreationDTO(
    [Required, StringLength(200, MinimumLength = 1)] string Street,
    [Required, StringLength(10, MinimumLength =1)] string Zipcode,
    [ Required, StringLength(100, MinimumLength =1)] string City,
    [ Required, StringLength(100,MinimumLength =1)] string State,
    [ Required, StringLength(50, MinimumLength =1)] string Country,
    [ Required] AddressType AddressType
    );
