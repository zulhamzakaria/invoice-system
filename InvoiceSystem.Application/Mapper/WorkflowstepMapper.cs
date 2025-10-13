using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Mapper;

public class WorkflowstepMapper
{
    public static Result<WorkflowStep> ToEntity(
        Guid invoiceId,
        InvoiceStatus statusBefore,
        InvoiceStatus statusAfter,
        WorkflowStepType actionType,
        Guid? approverId,
        string reason,
        DateTimeOffset timestamp)
    {
        return WorkflowStep.Create(
            invoiceId,
            statusBefore,
            statusAfter,
            actionType,
            approverId,
            reason,
            timestamp // The system-determined time
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
            entity.Timestamp // The recorded time is passed back to the client
        );
    }
}
