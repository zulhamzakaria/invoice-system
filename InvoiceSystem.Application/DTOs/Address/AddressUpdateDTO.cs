using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressUpdateDTO(
    [StringLength(200, MinimumLength = 1), NotEqual("string")] string ? Street,
    [StringLength(10, MinimumLength = 1), NotEqual("string")] string ? Zipcode,
    [StringLength(100, MinimumLength =1), NotEqual("string")] string ? City,
    [StringLength(100, MinimumLength = 1), NotEqual("string")] string ? State,
    [StringLength(50, MinimumLength = 1), NotEqual("string")] string ? Country
    );
