using InvoiceSystem.Application.DTOs.Invoice;
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

    public static InvoiceTaskDTO ToTaskDTO(Invoice invoice, WorkflowStep? step)
    {
        return new InvoiceTaskDTO(
            invoice.Id,
            invoice.InvoiceNumber,
            invoice.Status,
            step?.ActionType,
            step?.ApproverId ?? invoice.CreatedBy.Id,
            step?.CreatedAt ?? invoice.CreatedAt
        );
    }

    public static IReadOnlyList<InvoiceTaskDTO> ToTaskDTO(IEnumerable<Invoice> invoices, IEnumerable<WorkflowStep> steps)
    {
        var stepLookup = steps.ToDictionary(ws => ws.InvoiceId, ws => ws);
        return invoices.Select(inv =>
        ToTaskDTO(inv, stepLookup.ContainsKey(inv.Id) ? stepLookup[inv.Id] : null)).ToList();
    }
}
