using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class InvoiceOrchestratorService : IInvoiceOrchestratorService
{
    private readonly IUnitOfWork _uow;
    private readonly IInvoiceService _invoiceService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IWorkflowstepService _workflowstepService;
    private readonly ILoadTrackerService _loadTrackerService;
    public InvoiceOrchestratorService(IInvoiceService invoiceService,
        IWorkflowstepService workflowstepService,
        ILoadTrackerService loadTrackerService,
        IInvoiceRepository invoiceRepository,
        IEmployeeRepository employeeRepository,
        IWorkflowStepRepository workflowStepRepository,
        ILoadTrackerRepository loadTrackerRepository,
        IUnitOfWork uow)
    {
        _invoiceService = invoiceService;
        _workflowstepService = workflowstepService;
        _loadTrackerService = loadTrackerService;
        _invoiceRepository = invoiceRepository;
        _employeeRepository = employeeRepository;
        _uow = uow;
    }

    public async Task<Result> ApproveInvoiceAsync(Guid invoiceId, Guid approverId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }

        if (invoice.Status != InvoiceStatus.PendingOfficerApproval)
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
        var status = GetStatus(approvingOfficer);

        // status: approved
        var resultStep = await _workflowstepService.RecordStepAsync(
            invoice.Id, InvoiceStatus.PendingOfficerApproval, status, WorkflowStepType.Approval, approver.Id, "Approved Invoice",approverId);

        if (resultStep.IsFailure)
        {
            return Result.Failure(resultStep.Errors);
        }

        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RejectInvoiceAsync(Guid invoiceId, Guid employeeId, string reason)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }
        if (invoice.Status != InvoiceStatus.PendingOfficerApproval)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Approval.InvalidStatus, "Only Invoices pending for approval can be rejected"));
        }

        var approver = await _employeeRepository.GetByIdAsync(employeeId);
        if (approver is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));
        }
        if (approver is not IApprover approvingOfficer)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.InvalidApprover, "Only Approver can reject an Invoice"));
        }

        invoice.Reject(approver);
        // should be re-sent to the Clerk
        var resultStep = await _workflowstepService.RecordStepAsync(invoice.Id, InvoiceStatus.PendingOfficerApproval, InvoiceStatus.Rejected,
                                                                    WorkflowStepType.Approval, invoice.CreatedById, reason, approver.Id);
        if (resultStep.IsFailure)
        {
            return Result.Failure(resultStep.Errors);
        }
        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsCreationDTO dTO)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }
        var employee = await _employeeRepository.GetByIdAsync(dTO.EmployeeId);
        if (employee is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));
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

        var nextStatus = DetermineNextStatus(statusType, dTO.WorkflowStepType);

        var createWorkflowResult = await _workflowstepService.CreateWorkflowstepAsync(invoiceId, approver.Value.Id, dTO);
        if (createWorkflowResult.IsFailure)
        {
            return Result.Failure(createWorkflowResult.Errors);
        }
        //var submitResult = await _invoiceService.SubmitInvoiceAsync(invoiceId, employee);
        //if (submitResult.IsFailure)
        //{
        //    return Result.Failure(submitResult.Errors);
        //}

        // atomic save
        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> VoidInvoiceAsync(Guid invoiceid, Guid employeeid, string reason)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceid);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }
        var employee = await _employeeRepository.GetByIdAsync(employeeid);
        if (employee is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));
        }
        // no Reason added yet
        invoice.Void(employee);
        await _invoiceRepository.UpdateAsync(invoice);
        await _invoiceRepository.SaveChangesAsync();
        return Result.Success();
    }

    private InvoiceStatus GetStatus(IApprover approver)
    {
        return (approver) switch
        {
            FO => InvoiceStatus.PendingManagerApproval,
            FM => InvoiceStatus.Approved,
            _ => InvoiceStatus.PendingOfficerApproval,
        };
    }

    private InvoiceStatus DetermineNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType, IApprover approver = null)
    {
        return (currentStatus, stepType, approver) switch
        {
            (InvoiceStatus.Draft, WorkflowStepType.Submission, Clerk) => InvoiceStatus.PendingOfficerApproval,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval, FO) => InvoiceStatus.PendingManagerApproval,
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval, FM) => InvoiceStatus.Approved,
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.AutoApproval, _) => InvoiceStatus.Approved,

            (_, WorkflowStepType.Rejection, _) => InvoiceStatus.Rejected,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Routing, _) => InvoiceStatus.PendingOfficerApproval,
            (InvoiceStatus.Approved, WorkflowStepType.PaymentProcessing, _) => InvoiceStatus.Paid,

            (_, WorkflowStepType.Recall, _) => InvoiceStatus.Draft,
            (_, WorkflowStepType.Delegation, _) => currentStatus,
            (_, WorkflowStepType.Escalation, _) => currentStatus,

            _ => currentStatus
        };
    }


}
