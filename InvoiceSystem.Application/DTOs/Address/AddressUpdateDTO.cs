using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressUpdateDTO(
    [StringLength(200, MinimumLength = 1)]
    string ? Street,
    [property: StringLength(10, MinimumLength =1)]
    string ? Zipcode,
    [property: StringLength(100, MinimumLength =1)]
    string ? City,
    [property : StringLength(100, MinimumLength = 1)]
    string ? State,
    [property : StringLength(50, MinimumLength = 1)]
    string ? Country);
