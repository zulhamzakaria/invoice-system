using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemsDelete([Required]Guid EmployeeId, [Required, MinLength(1)] IEnumerable<Guid> ItemIds);
