using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Domain.Repositories;
using static InvoiceSystem.Domain.Errors.InvoiceErrors;

namespace InvoiceSystem.Application.Services;

public class InvoiceOrchestratorService : IInvoiceOrchestratorService
{
    private readonly IUnitOfWork _uow;
    private readonly IInvoiceService _invoiceService;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IWorkflowstepService _workflowstepService;
    private readonly ILoadTrackerService _loadTrackerService;
    public InvoiceOrchestratorService(IInvoiceService invoiceService,
        IWorkflowstepService workflowstepService,
        ILoadTrackerService loadTrackerService,
        IInvoiceRepository invoiceRepository,
        IEmployeeRepository employeeRepository,
        ILoadTrackerRepository loadTrackerRepository,
        IUnitOfWork uow,
        IEmployeeService employeeService)
    {
        _invoiceService = invoiceService;
        _workflowstepService = workflowstepService;
        _loadTrackerService = loadTrackerService;
        _invoiceRepository = invoiceRepository;
        _employeeRepository = employeeRepository;
        _uow = uow;
        _employeeService = employeeService;
    }

    public async Task<Result> ApproveInvoiceAsync(Guid invoiceId, Guid approverId)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }

        if (invoice.Status != InvoiceStatus.PendingOfficerApproval && invoice.Status != InvoiceStatus.PendingManagerApproval)
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

        //can only Approve the designated Invoices
        //call FROM the workflowstep service
        var assignedInvoices = await _workflowstepService.GetInvoicesByApproverId(approverId);
        if (assignedInvoices is null || !assignedInvoices.Any())
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.NoAssignedInvoice, "Provided Approver has no assigned Invoices"));
        }
        bool exists = assignedInvoices.Contains(invoiceId);
        if (!exists)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.NotAssignedInvoice, "This Approver cannot act on this invoice"));
        }

        //NOT GETTING NEXT APPROVER
        //get the sole FM. 
        var FM = await _employeeService.GetEmployeesByType(EmployeeType.FM);
        if (FM.IsFailure)
        {
            return Result.Failure(FM.Errors);
        }

        var nextStatus = DetermineNextStatus(invoice.Status, WorkflowStepType.Approval, approvingOfficer);
        //TODO: check the value here later
        var nextStatus2 = WorkflowStepStateRules.DetermineNextStatus(invoice.Status, WorkflowStepType.Approval, approvingOfficer.EmployeeType);

        //workflowstep follows current invoice.status.
        //then invoice.status gets updated
        var resultStep = await _workflowstepService.RecordStepAsync(
            invoice.Id, invoice.Status, nextStatus, WorkflowStepType.Approval, FM.Value.FirstOrDefault().Id,
            $"{approver.GetType().Name} Approved Invoice", approverId);

        if (resultStep.IsFailure)
        {
            return Result.Failure(resultStep.Errors);
        }

        invoice.Approve(approver, approvingOfficer.MaxApprovalAmount, nextStatus);

        //Atomic save
        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RejectInvoiceAsync(Guid invoiceId, WorkflowstepsActionDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            return Result.Failure(Error.Validation(WorkflowStepErrors.Creation.MissingReason, "Reason must be provided to Reject an Invoice"));
        }
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }
        if (invoice.Status != InvoiceStatus.PendingOfficerApproval)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Approval.InvalidStatus, "Only Invoices pending for approval can be rejected"));
        }

        var approver = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (approver is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));
        }
        if (approver is not IApprover approvingOfficer)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.InvalidApprover, "Only Approver can reject an Invoice"));
        }

        //can only Reject the designated Invoices
        //call FROM the workflowstep service
        var assignedInvoices = await EnsureInvoiceOwnershipAsync(approver.Id, invoiceId);
        if (assignedInvoices.IsFailure)
        {
            return Result.Failure(assignedInvoices.Errors);
        }

        //should be re-sent back to the Clerk (Invoice Creator)
        var clerkId = invoice.CreatedById;
        //using CreateWorkflowStepAsync 
        WorkflowstepsCreationDTO creationDTO =
            new WorkflowstepsCreationDTO(WorkflowStepType.Rejection, dto.EmployeeId, approvingOfficer.EmployeeType, dto.Reason);
        var createWorkflowResult = await _workflowstepService.CreateWorkflowstepAsync(invoiceId, clerkId, creationDTO);

        if (createWorkflowResult.IsFailure)
        {
            return Result.Failure(createWorkflowResult.Errors);
        }

        try
        {
            invoice.Reject(approver);
        }
        catch (DomainException ex)
        {
            return Result.Failure(Error.Validation(ex.ErrorCode, ex.Message));
        }

        //await _invoiceRepository.UpdateAsync(invoice);
        //await _invoiceRepository.SaveChangesAsync();
        await _uow.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> SubmitInvoiceAsync(Guid invoiceId, WorkflowstepsActionDTO dTO)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }

        var status = Enum.Parse<InvoiceStatus>(invoice.Value.Status);
        if (!InvoiceStatusRules.CanSubmit.Contains(status))
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvalidStatus, "Only Draft/Rejected Invoice can be submitted"));
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

        //var nextStatus = DetermineNextStatus(statusType, dTO.WorkflowStepType);
        WorkflowstepsCreationDTO creationDTO =
            new WorkflowstepsCreationDTO(WorkflowStepType.Submission, dTO.EmployeeId, EmployeeType.Clerk, dTO.Reason);
        var createWorkflowResult = await _workflowstepService.CreateWorkflowstepAsync(invoiceId, approver.Value.Id, creationDTO);
        if (createWorkflowResult.IsFailure)
        {
            return Result.Failure(createWorkflowResult.Errors);
        }

        var submitResult = await _invoiceService.SubmitInvoiceAsync(invoiceId, employee);
        if (submitResult.IsFailure)
        {
            return Result.Failure(submitResult.Errors);
        }

        // atomic save
        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> VoidInvoiceAsync(Guid invoiceId, WorkflowstepsActionDTO dto)
    {
        var invoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
        if (invoice is null)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
        }
        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            return Result.Failure(Error.Validation(WorkflowStepErrors.Creation.MissingReason, "Reason must be provided to Void an Invoice"));
        }
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
        if (employee is null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee found"));
        }
        //TODO: invoice ownership check
        if (invoice.Value.CreatedById != dto.EmployeeId)
        {

        }

        //calls the InvoiceService Void() instead
        var voidInvoice = await _invoiceService.VoidInvoiceAsync(invoice.Value.Id, employee);
        if (voidInvoice.IsFailure)
        {
            return Result.Failure(voidInvoice.Errors);
        }

        //TODO: Workflowstep for Void
        WorkflowstepsCreationDTO creationDTO =
            new WorkflowstepsCreationDTO(WorkflowStepType.Void, dto.EmployeeId, EmployeeType.Clerk, dto.Reason);
        var createWorkflowResult = await _workflowstepService.CreateWorkflowstepAsync(invoiceId, dto.EmployeeId, creationDTO);
        if (createWorkflowResult.IsFailure)
        {
            return Result.Failure(createWorkflowResult.Errors);
        }

        //atomic save cause we're calling both Invoice and WorkflowStep
        await _uow.SaveChangesAsync();

        return Result.Success();
    }

    private InvoiceStatus DetermineNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType, IApprover approver = null)
    {
        return (currentStatus, stepType, approver) switch
        {
            (InvoiceStatus.Draft, WorkflowStepType.Submission, Clerk) => InvoiceStatus.PendingOfficerApproval,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval, FO) => InvoiceStatus.PendingManagerApproval,
            (InvoiceStatus.PendingManagerApproval, WorkflowStepType.Approval, FM) => InvoiceStatus.ApprovedByManager,
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.AutoApproval, _) => InvoiceStatus.ApprovedByManager,

            // REJECTIONS — only valid at approval stages
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Rejection, FO) => InvoiceStatus.Rejected,
            (InvoiceStatus.PendingManagerApproval, WorkflowStepType.Rejection, FM) => InvoiceStatus.Rejected,

            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Routing, _) => InvoiceStatus.PendingOfficerApproval,
            (InvoiceStatus.ApprovedByManager, WorkflowStepType.PaymentProcessing, _) => InvoiceStatus.Paid,

            (_, WorkflowStepType.Recall, _) => InvoiceStatus.Draft,
            (_, WorkflowStepType.Delegation, _) => currentStatus,
            (_, WorkflowStepType.Escalation, _) => currentStatus,

            _ => currentStatus
        };
    }

    private async Task<Result> EnsureInvoiceOwnershipAsync(Guid approverId, Guid invoiceId)
    {
        var assignedInvoices = await _workflowstepService.GetInvoicesByApproverId(approverId);
        if (assignedInvoices is null || !assignedInvoices.Any())
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.NoAssignedInvoice, "Provided Approver has no assigned Invoices"));
        }
        bool exists = assignedInvoices.Contains(invoiceId);
        if (!exists)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.NotAssignedInvoice, "This Approver cannot act on this invoice"));
        }
        return Result.Success();
    }

}
