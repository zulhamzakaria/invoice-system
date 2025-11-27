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

    public WorkflowstepService(
        IWorkflowStepRepository workflowStepRepository,
        IInvoiceRepository invoiceRepository)
    {
        _workflowStepRepository = workflowStepRepository;
        _invoiceRepository = invoiceRepository;
    }
    public async Task<Result<WorkflowstepsDetailsDTO>> CreateWorkflowstepAsync(Guid invoiceId, Guid approverId, WorkflowstepsCreationDTO dto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            var errors = new List<Error> { Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No invoice found for the Invoice Id") };
            return Result<WorkflowstepsDetailsDTO>.Failure(errors);
        }

        var statusBefore = invoice.Status;
        var statusAfter = DetermineNextStatus(invoice.Status, dto.WorkflowStepType);
        // TODO: might wanna test this later
        var statusAfter2 = WorkflowStepStateRules.DetermineNextStatus(invoice.Status, dto.WorkflowStepType, EmployeeType.Clerk);
        var timestamp = DateTimeOffset.UtcNow;

        var stepResult = WorkflowstepMapper.ToEntity(
            invoiceId,
            statusBefore,
            statusAfter,
            dto.WorkflowStepType,
            approverId,
            dto.Reason ?? "",
            timestamp,
            dto.EmployeeId
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

    public async Task<IReadOnlyList<Guid?>> GetInvoicesByApproverId(Guid approverId)
    {
        return await _workflowStepRepository.GetByApproverIdAsync(approverId);
    }

    public async Task<Result> RecordStepAsync(Guid invoiceId,
                                        InvoiceStatus before,
                                        InvoiceStatus after,
                                        WorkflowStepType stepType,
                                        Guid? approverId,
                                        string reason, Guid createdBy)
    {
        var stepResult = WorkflowStep.Create(invoiceId, before, after, stepType, approverId, reason, DateTimeOffset.UtcNow, createdBy);

        if (stepResult.IsFailure)
        {
            return Result.Failure(stepResult.Errors);
        }

        await _workflowStepRepository.AddAsync(stepResult.Value);
        //await _workflowStepRepository.SaveChangesAsync();
        return Result.Success();

    }

    private InvoiceStatus DetermineNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType)
    {
        return (currentStatus, stepType) switch
        {
            (InvoiceStatus.Draft, WorkflowStepType.Submission) => InvoiceStatus.PendingOfficerApproval,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval) => InvoiceStatus.PendingManagerApproval,
            (InvoiceStatus.PendingManagerApproval, WorkflowStepType.Approval) => InvoiceStatus.ApprovedByManager,
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.AutoApproval) => InvoiceStatus.ApprovedByManager,

            // REJECTIONS — only valid at approval stages
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Rejection) => InvoiceStatus.Rejected,
            (InvoiceStatus.PendingManagerApproval, WorkflowStepType.Rejection) => InvoiceStatus.Rejected,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Routing) => InvoiceStatus.PendingOfficerApproval,
            (InvoiceStatus.ApprovedByManager, WorkflowStepType.PaymentProcessing) => InvoiceStatus.Paid,

            (_, WorkflowStepType.Recall) => InvoiceStatus.Draft,
            (_, WorkflowStepType.Delegation) => currentStatus,
            (_, WorkflowStepType.Escalation) => currentStatus,

            _ => currentStatus
        };
    }

}
