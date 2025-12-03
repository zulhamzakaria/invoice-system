using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceTaskDTO(
        Guid InvoiceId,
        string InvoiceNo,
        InvoiceStatus CurrentStatus,
        WorkflowStepType? StepType,  // null for Drafts
        Guid AssignedToId,           // ClerkId or ApproverId
        DateTime CreatedAt
    );
