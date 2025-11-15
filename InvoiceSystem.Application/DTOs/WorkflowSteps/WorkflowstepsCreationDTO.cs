using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.WorkflowSteps;

public record WorkflowstepsCreationDTO(
    [Required] Guid InvoiceId,
    [Required] WorkflowStepType WorkflowStepType,
    Guid? ApproverId,
    [Required, StringLength(500, MinimumLength = 1)] string? Reason
    );
