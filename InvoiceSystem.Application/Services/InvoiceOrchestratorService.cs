using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;

namespace InvoiceSystem.Application.Services;

public class InvoiceOrchestratorService : IInvoiceOrchestratorService
{
    private readonly IInvoiceService _invoiceService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IWorkflowstepService _workflowstepService;
    private readonly ILoadTrackerService _loadTrackerService;
    public InvoiceOrchestratorService(IInvoiceService invoiceService,
        IWorkflowstepService workflowstepService,
        ILoadTrackerService loadTrackerService,
        IInvoiceRepository invoiceRepository,
        IEmployeeRepository employeeRepository)
    {
        _invoiceService = invoiceService;
        _workflowstepService = workflowstepService;
        _loadTrackerService = loadTrackerService;
        _invoiceRepository = invoiceRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> ApproveInvoiceAsync(Guid invoiceId, Guid approverId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }

        if (invoice.Status != InvoiceStatus.PendingApproval)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Approval.InvalidStatus, "Only Invoices pending for approval can be approved"));
        }

        var approver = await _employeeRepository.GetByIdAsync(approverId);
        if (approver is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));

        }
        if (approver is not IApprover approvingOfficer)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.InvalidApprover, "Provided Employee is not a valid Approver"));
        }
        invoice.Approve(approver, approvingOfficer.MaxApprovalAmount);

        // status: approved
        var resultStep = await _workflowstepService.RecordStepAsync(
            invoice.Id, InvoiceStatus.PendingApproval, InvoiceStatus.Approved, WorkflowStepType.Approval, approver.Id, "Approved Invoice");

        if (resultStep.IsFailure)
        {
            return Result.Failure(resultStep.Errors);
        }

        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return Result.Success();
    }

    public Task<Result> RejectInvoiceAsync(Guid invoiceId, Guid employeeId, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsCreationDTO dTO)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if (invoice is null)
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
        if (!Enum.TryParse<InvoiceStatus>(invoice.Value.Status, out var statusType))
        {
            return Result.Failure(Error.Validation("InvoiceStatusUndefined", "Invalid Invoice status"));
        }

        var nextStatus = DetermineNextStatus(statusType, dTO.ActionType);
        await _workflowstepService.CreateWorkflowstepAsync(dTO);
        await _invoiceService.UpdateInvoiceStatusAsync(invoice.Value.Id, nextStatus);
        return Result.Success();


    }

    private InvoiceStatus DetermineNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType)
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
