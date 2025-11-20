using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IWorkflowstepService
{
    Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(Guid invoiceId, WorkflowstepsCreationDTO dto);

    Task<Result> RecordStepAsync(Guid invoiceId,
                                InvoiceStatus before,
                                InvoiceStatus after,
                                WorkflowStepType stepType,
                                Guid? approverId,
                                string reason, Guid createdBy);
}
