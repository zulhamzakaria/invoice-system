using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IWorkflowstepService
{
    Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(WorkflowstepsCreationDTO dto);
}
