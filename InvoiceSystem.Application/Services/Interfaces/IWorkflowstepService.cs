using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IWorkflowstepService
{
    Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(Guid invoiceId, Guid approverId, WorkflowstepsCreationDTO dto);
    Result<IReadOnlyList<InvoiceApproverTaskDTO>> GetApproverTasks(Employee employee);
    Task<IReadOnlyList<Guid?>> GetInvoicesByApproverId(Guid approverId);
    Task<Result> RecordStepAsync(Guid invoiceId,
                                InvoiceStatus before,
                                InvoiceStatus after,
                                WorkflowStepType stepType,
                                Guid? approverId,
                                string reason, Guid createdBy);
}
