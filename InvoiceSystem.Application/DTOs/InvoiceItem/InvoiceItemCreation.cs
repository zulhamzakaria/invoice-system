using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemCreation(
    [property: Required, StringLength(500, MinimumLength =1)]
    string Description,
    [property: Required, Range(0, int.MaxValue)]
    int Quantity,
    [property: Required, Range(0.0001, int.MaxValue)]
    decimal UnitPrice
    );
