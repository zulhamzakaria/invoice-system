using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemsDeleteDTO([Required]Guid EmployeeId, [Required, MinLength(1)] IEnumerable<Guid> ItemIds);
