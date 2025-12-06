using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemUpdateDTO(
    [StringLength(500, MinimumLength =1), NotEqual("string")] string? Description,
    [Range(0, int.MaxValue)] int? Quantity,
    [Range(0.0001, int.MaxValue)] decimal? UnitPrice
    );