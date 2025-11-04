using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Application.Services;

public class InvoiceOrchestratorService : IInvoiceOrchestratorService
{
    private readonly IInvoiceService _invoiceService;
    private readonly IWorkflowstepService _workflowstepService;
    private readonly ILoadTrackerService _loadTrackerService;
    public InvoiceOrchestratorService(IInvoiceService invoiceService, 
        IWorkflowstepService workflowstepService, 
        ILoadTrackerService loadTrackerService)
    {
        _invoiceService = invoiceService;
        _workflowstepService = workflowstepService;
        _loadTrackerService = loadTrackerService;
    }
    public async Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsCreationDTO dTO)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if(invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }

        var approver = await _loadTrackerService.GetNextApproverAsync(invoice.Value.InvoiceAmount);
        if (approver.IsFailure)
        {
            return Result.Failure(approver.Errors);
        }

        await _loadTrackerService.RecordAssignmentAsync(approver.Value.Id);

        //convert status to Enum
        if(!Enum.TryParse<InvoiceStatus>(invoice.Value.Status, out var statusType))
        {
            return Result.Failure(Error.Validation("InvoiceStatusUndefined", "Invalid Invoice status"));
        }

        var nextStatus = DetermineNextStatus(statusType, dTO.ActionType);
        await _workflowstepService.CreateWorkflowstepAsync(dTO);
        await _invoiceService.UpdateInvoiceAsync(invoice.Value.Id, nextStatus);
        return Result.Success();


    }

    private InvoiceStatus DetermineNextStatus (InvoiceStatus currentStatus, WorkflowStepType stepType)
    {
        return (currentStatus, stepType) switch
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
