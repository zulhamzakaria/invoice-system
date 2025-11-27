using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IInvoiceOrchestratorService
{
    Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsActionDTO dto);
    Task<Result> ApproveInvoiceAsync(Guid invoiceId, Guid approverId);
    Task<Result> RejectInvoiceAsync(Guid invoiceId, WorkflowstepsActionDTO dto);
    Task<Result> VoidInvoiceAsync(Guid invoiceid, WorkflowstepsActionDTO dto);
}
