using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.WorkflowSteps;

public record WorkflowstepsCreationDTO(
    [Required]
    Guid InvoiceId,
    [Required]
    InvoiceStatus StatusBefore,
    [Required]
    InvoiceStatus StatusAfter,
    [Required]
    WorkflowStepType ActionType,
    Guid? ApproverId,
    [property: Required, StringLength(500, MinimumLength = 1)]
    string Reason
    );
