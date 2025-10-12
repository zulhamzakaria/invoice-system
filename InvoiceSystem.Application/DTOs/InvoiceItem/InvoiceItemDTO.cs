namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemDTO(
    Guid Id,
    string? Description,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
    );

