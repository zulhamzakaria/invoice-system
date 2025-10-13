namespace InvoiceSystem.Application.DTOs.WorkflowSteps;

public record WorkflowstepsDetailsDTO(
    Guid Id,
    Guid InvoiceId,
    string StatusBefore,
    string StatusAfter,
    string ActionType,
    Guid? ApproverId,
    string Reason,
    DateTimeOffset Timestamp
    );
