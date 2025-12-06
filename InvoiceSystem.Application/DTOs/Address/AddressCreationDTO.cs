using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressCreationDTO(
    [Required, StringLength(200, MinimumLength = 1), NotEqual("string")] string Street,
    [Required, StringLength(10, MinimumLength = 1), NotEqual("string")] string Zipcode,
    [Required, StringLength(100, MinimumLength =1), NotEqual("string")] string City,
    [Required, StringLength(100, MinimumLength = 1), NotEqual("string")] string State,
    [Required, StringLength(50, MinimumLength = 1), NotEqual("string")] string Country
    //[Required] AddressType AddressType
    );
