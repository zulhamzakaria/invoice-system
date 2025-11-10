using System.ComponentModel.DataAnnotations;
namespace InvoiceSystem.Application.DTOs.InvoiceOrchestrator;

public record InvoiceRejectionDTO(
    [Required]
    Guid employeeId,
    [property: Required, StringLength(500, MinimumLength = 1)]
    string Reason
    );
