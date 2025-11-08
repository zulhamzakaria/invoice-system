using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceOrchestrator;

public record InvoiceRejectionDTO(
    [Required]
    Guid invoiceId,
    [property: Required, StringLength(500, MinimumLength = 1)]
    string Reason
    );
