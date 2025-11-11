using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Address;

public record AddressUpdateDTO(
    [StringLength(200, MinimumLength = 1)] string ? Street,
    [StringLength(10, MinimumLength =1)] string ? Zipcode,
    [StringLength(100, MinimumLength =1)] string ? City,
    [StringLength(100, MinimumLength = 1)] string ? State,
    [StringLength(50, MinimumLength = 1)] string ? Country
    );
