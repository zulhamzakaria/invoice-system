using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemDeleteDTO([Required] Guid EmployeeId,
                                   [Required] Guid ItemId);

