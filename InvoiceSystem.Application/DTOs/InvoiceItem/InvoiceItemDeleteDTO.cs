using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceItem;

public record InvoiceItemDeleteDTO([Required]Guid ItemId, 
                                   [Required]Guid EmployeeId);

