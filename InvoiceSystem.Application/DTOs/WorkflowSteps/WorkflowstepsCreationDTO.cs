using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.WorkflowSteps;

public record WorkflowstepsCreationDTO(
    ///<summary>ApproverId and Reason are not required by Submission</summary>
    [Required] WorkflowStepType WorkflowStepType,
    Guid? ApproverId,
    [StringLength(500, MinimumLength = 1)] string? Reason
    );
