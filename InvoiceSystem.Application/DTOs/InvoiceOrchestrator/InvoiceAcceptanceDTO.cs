using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceOrchestrator;

public record InvoiceAcceptanceDTO(
    [Required]
    Guid approverId,
    [StringLength(maximumLength:500, MinimumLength =1)]
    string? Remarks);
