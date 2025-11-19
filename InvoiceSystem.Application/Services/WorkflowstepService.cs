using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Mappers;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
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
    public async Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(Guid invoiceId, WorkflowstepsCreationDTO dto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            var errors = new List<Error> { Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No invoice found for the Invoice Id") };
            return Result<WorkflowstepsDetailsDTO>.Failure(errors);
        }

        //LoadTracker
        var approver = await _loadTrackerService.GetNextApproverAsync(invoice.TotalAmount);
        if (approver.IsFailure)
        {
            return Result<WorkflowstepsDetailsDTO>.Failure(approver.Errors);
        }

        await _loadTrackerService.RecordAssignmentAsync(approver.Value.Id);

        var statusBefore = invoice.Status;
        var statusAfter = DeterminNextStatus(invoice.Status, dto.WorkflowStepType);
        var timestamp = DateTimeOffset.UtcNow;

        var stepResult = WorkflowstepMapper.ToEntity(
            invoiceId,
            statusBefore,
            statusAfter,
            dto.WorkflowStepType,
            dto.EmployeeId,
            dto.Reason ?? "",
            timestamp
            );

        if (stepResult.IsFailure)
        {
            return Result<WorkflowstepsDetailsDTO>.Failure(stepResult.Errors);
        }
        var newStep = stepResult.Value;
        await _workflowStepRepository.AddAsync(newStep);
        //await _workflowStepRepository.SaveChangesAsync();
        return Result<WorkflowstepsDetailsDTO>.Success(WorkflowstepMapper.ToDetailsDTO(newStep));
    }

    public async Task<Result> RecordStepAsync(Guid invoiceId,
                                        InvoiceStatus before,
                                        InvoiceStatus after,
                                        WorkflowStepType stepType,
                                        Guid? approverId,
                                        string reason)
    {
        var stepResult = WorkflowStep.Create(invoiceId, before, after, stepType, approverId, reason, DateTimeOffset.UtcNow);

        if (stepResult.IsFailure)
        {
            return Result.Failure(stepResult.Errors);
        }

        await _workflowStepRepository.AddAsync(stepResult.Value);
        //await _workflowStepRepository.SaveChangesAsync();
        return Result.Success();

    }

    private InvoiceStatus DeterminNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType)
    {
        return (currentStatus, stepType) switch
        {
            (InvoiceStatus.Draft, WorkflowStepType.Submission) => InvoiceStatus.PendingOfficerApproval,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval) => InvoiceStatus.Approved,
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.AutoApproval) => InvoiceStatus.Approved,

            (_, WorkflowStepType.Rejection) => InvoiceStatus.Rejected,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Routing) => InvoiceStatus.PendingOfficerApproval,
            (InvoiceStatus.Approved, WorkflowStepType.PaymentProcessing) => InvoiceStatus.Paid,

            (_, WorkflowStepType.Recall) => InvoiceStatus.Draft,
            (_, WorkflowStepType.Delegation) => currentStatus,
            (_, WorkflowStepType.Escalation) => currentStatus,

            _ => currentStatus
        };
    }

}
