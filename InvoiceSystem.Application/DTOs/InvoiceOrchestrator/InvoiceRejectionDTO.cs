using System.ComponentModel.DataAnnotations;
namespace InvoiceSystem.Application.DTOs.InvoiceOrchestrator;

public record InvoiceRejectionDTO(
    [Required] Guid employeeId,
    [Required, StringLength(500, MinimumLength = 1)] string reason
    );
