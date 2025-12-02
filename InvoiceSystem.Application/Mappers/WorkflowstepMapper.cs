using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Mappers;

public class WorkflowstepMapper
{
    public static Result<WorkflowStep> ToEntity(
        Guid invoiceId,
        InvoiceStatus statusBefore,
        InvoiceStatus statusAfter,
        WorkflowStepType actionType,
        Guid? approverId,
        string reason,
        DateTimeOffset timestamp, Guid createdById)
    {
        return WorkflowStep.Create(
            invoiceId,
            statusBefore,
            statusAfter,
            actionType,
            approverId,
            reason,
            timestamp, createdById // The system-determined time
        );
    }

    public static WorkflowstepsDetailsDTO ToDetailsDTO(WorkflowStep entity)
    {
        return new WorkflowstepsDetailsDTO(
            entity.Id,
            entity.InvoiceId,
            entity.StatusBefore.ToString(),
            entity.StatusAfter.ToString(),
            entity.ActionType.ToString(),
            entity.ApproverId,
            entity.Reason,
            entity.Timestamp
        );
    }

    public static WorkflowstepHistoryDTO ToHistoryDTO(WorkflowStep workflowStep)
    {
        return new WorkflowstepHistoryDTO(
            workflowStep.StatusBefore,
            workflowStep.StatusAfter,
            workflowStep.ActionType,
            workflowStep.ApproverId,
            workflowStep.Reason ?? "",
            workflowStep.Timestamp);
    }

    public static IReadOnlyList<WorkflowstepHistoryDTO> ToHistoryDTO(IEnumerable<WorkflowStep> workflowSteps)
    {
        return workflowSteps.Select(ToHistoryDTO).ToList();
    }
}
