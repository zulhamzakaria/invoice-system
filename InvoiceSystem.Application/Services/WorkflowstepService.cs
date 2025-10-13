using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class WorkflowstepService : IWorkflowstepService
{
    private readonly IWorkflowStepRepository _workflowStepRepository;
    private readonly IInvoiceRepository _invoiceRepository;

    public WorkflowstepService(IWorkflowStepRepository workflowStepRepository, IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
        _workflowStepRepository = workflowStepRepository;
    }
    public async Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(WorkflowstepsCreationDTO dto)
    {
        var invoice  = await _invoiceRepository.GetByIdAsync(dto.InvoiceId);
        if (invoice == null)
        {
            var errors = new List<Error> { Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No invoice found for the Invoice Id")};
            return Result<WorkflowstepsDetailsDTO>.Failure(errors);
        }
        var statusBefore = invoice.Status;
        var timestamp = DateTimeOffset.UtcNow;
    }

    private InvoiceStatus DeterminNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType) {

        switch (stepType)
        {
            case WorkflowStepType.Approval:
                return InvoiceStatus.Approved;
            case WorkflowStepType.Rejection:
                return InvoiceStatus.Rejected;
            case WorkflowStepType.Routing:
                return currentStatus;
            case WorkflowStepType.PaymentProcessing:
                return InvoiceStatus.Paid;
             default: return currentStatus;
        }
    }

}
