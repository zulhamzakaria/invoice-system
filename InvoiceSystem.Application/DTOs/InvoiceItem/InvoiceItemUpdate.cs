using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemUpdate(
    [property:  StringLength(500, MinimumLength =1)]
    string? Description,
    [property:  Range(0, int.MaxValue)]
    int? Quantity,
    [property:  Range(0.0001, int.MaxValue)]
    decimal? UnitPrice
    );