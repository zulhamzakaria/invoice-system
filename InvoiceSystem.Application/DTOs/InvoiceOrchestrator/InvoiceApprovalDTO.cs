using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.InvoiceOrchestrator;

public record InvoiceApprovalDTO(
    [Required] Guid approverId
    //[StringLength(maximumLength:500, MinimumLength =1)] string? Remarks
    );
