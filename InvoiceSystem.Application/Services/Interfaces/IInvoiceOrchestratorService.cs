using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IInvoiceOrchestratorService
{
    Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsCreationDTO dTO);
    Task<Result> ApproveInvoiceAsync(Guid invoiceId, Guid approverId);
    Task<Result> RejectInvoiceAsync(Guid invoiceId, Guid employeeId, string reason);
    Task<Result> VoidInvoiceAsync(Guid invoiceid, Guid employeeid, string reason);
}
