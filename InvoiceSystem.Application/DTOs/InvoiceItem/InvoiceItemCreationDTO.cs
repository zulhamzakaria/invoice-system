using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemCreationDTO(
    [Required, NotEqual("string")] string Description,
    [Required, NotEqual("string")] int Quantity,
    [Required, NotEqual("string")] decimal UnitPrice
    );
