using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.DTOs.WorkflowSteps;

public record WorkflowstepHistoryDTO(
        InvoiceStatus StatusBefore,
        InvoiceStatus StatusAfter,
        WorkflowStepType ActionType,
        Guid? ApproverId,
        string? Reason,
        DateTimeOffset Timestamp
    );
