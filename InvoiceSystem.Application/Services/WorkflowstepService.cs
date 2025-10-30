using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Mappers;
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
    private readonly ILoadTrackerService _loadTrackerService;

    public WorkflowstepService(
        IWorkflowStepRepository workflowStepRepository, 
        IInvoiceRepository invoiceRepository, 
        ILoadTrackerService loadTrackerService)
    {
        _workflowStepRepository = workflowStepRepository;
        _invoiceRepository = invoiceRepository;
        _loadTrackerService = loadTrackerService;
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
        var statusAfter = DeterminNextStatus(invoice.Status, dto.ActionType);
        var timestamp = DateTimeOffset.UtcNow;

        var stepResult = WorkflowstepMapper.ToEntity(
            dto.InvoiceId,
            statusBefore,
            statusAfter,
            dto.ActionType,
            dto.ApproverId,
            dto.Reason,
            timestamp
            );

        if (stepResult.IsFailure)
        {
            return Result<WorkflowstepsDetailsDTO>.Failure(stepResult.Errors);        
        }
        var newStep = stepResult.Value;
        await _workflowStepRepository.AddAsync(newStep);
        await _workflowStepRepository.SaveChangesAsync();
        return Result<WorkflowstepsDetailsDTO>.Success(WorkflowstepMapper.ToDetailsDTO(newStep));
    }

    private InvoiceStatus DeterminNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType) {
        return (currentStatus,  stepType) switch
        {
            (InvoiceStatus.PendingApproval, WorkflowStepType.Approval) => InvoiceStatus.Approved,
            (InvoiceStatus.PendingApproval, WorkflowStepType.AutoApproval) => InvoiceStatus.Approved,

            (_, WorkflowStepType.Rejection) => InvoiceStatus.Rejected,

            (InvoiceStatus.PendingApproval, WorkflowStepType.Routing) => InvoiceStatus.PendingApproval,
            (InvoiceStatus.Approved, WorkflowStepType.PaymentProcessing) => InvoiceStatus.Paid,

            (_, WorkflowStepType.Recall) => InvoiceStatus.Draft,
            (_, WorkflowStepType.Delegation) => currentStatus,
            (_, WorkflowStepType.Escalation) => currentStatus,

            _ => currentStatus
        };
    }

}
