using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceApproverTaskDTO(
        Guid InvoiceId,
        string InvoiceNo,
        InvoiceStatus CurrentStatus,
        WorkflowStepType? StepType,  // null for Drafts
        Guid? AssignedToId,           // ClerkId or ApproverId
        DateTimeOffset? CreatedAt
    );
