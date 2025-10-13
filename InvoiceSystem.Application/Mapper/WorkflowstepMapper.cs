using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mapper;

public class WorkflowstepMapper
{
    public static WorkflowStep ToEntity(WorkflowstepsCreationDTO dto)
    {
        var step = WorkflowStep.Create(
            dto.InvoiceId,
            dto.StatusBefore,
            dto.StatusAfter,
            dto.ActionType,
            dto.ApproverId,
            dto.Reason
            );
       
    }
}
