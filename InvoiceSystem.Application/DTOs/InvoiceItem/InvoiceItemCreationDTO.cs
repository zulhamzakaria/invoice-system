using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemCreationDTO(
    [Required] string Description,
    [Required] int Quantity,
    [Required] decimal UnitPrice
    );
