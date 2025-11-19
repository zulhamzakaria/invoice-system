using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class WorkflowStep
{
    private const int MaxReasonlength = 500;

    // Properties are read-only or use private setters to enforce immutability
    public Guid Id { get; }
    public Guid InvoiceId { get; }
    public InvoiceStatus StatusBefore { get; }
    public InvoiceStatus StatusAfter { get; }
    public WorkflowStepType ActionType { get; } // Renamed from StepType to ActionType for clarity
    public Guid? ApproverId { get; private set; } // Nullable, as the system can perform actions (Routing)
    public string Reason { get; }
    public DateTimeOffset Timestamp { get; init; }

    private WorkflowStep() { } //For EF Core
    // Private Constructor (Enforcing Validity)
    private WorkflowStep(
        Guid id,
        Guid invoiceId,
        InvoiceStatus statusBefore,
        InvoiceStatus statusAfter,
        WorkflowStepType actionType,
        Guid? approverId,
        string reason,
        DateTimeOffset timestamp)
    {
        // Assignment to read-only properties is allowed here
        Id = id;
        InvoiceId = invoiceId;
        StatusBefore = statusBefore;
        StatusAfter = statusAfter;
        ActionType = actionType;
        ApproverId = approverId;
        Reason = reason;
        Timestamp = timestamp;
    }

    // The Static Factory Method (Making the Entity Rich)
    public static Result<WorkflowStep> Create(
        Guid invoiceId,
        InvoiceStatus statusBefore,
        InvoiceStatus statusAfter,
        WorkflowStepType actionType,
        Guid? approverId,
        string reason,
        DateTimeOffset timestamp)
    {
        // --- 1. Centralized Invariant and Validation Checks ---
        string trimmedReason = reason?.Trim() ?? string.Empty;
        var errors = new List<Error>();

        // Check 1: Mandatory Fields
        if (invoiceId == Guid.Empty)
        {
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.InvalidInvoiceId, "Invoice ID cannot be empty."));
        }

        // Check 2: Logical Invariant (Must be a change)
        if (statusBefore == statusAfter && actionType != WorkflowStepType.Routing)
        {
            // Allow Routing to have the same status (e.g., PendingApproval -> PendingApproval) 
            // but not manual actions, as a manual action must result in a decision (Approval/Rejection).
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.NoStatusChange,
                "Manual action must result in a status change (or be a Routing action)."));
        }

        // Check 3: Actor Invariant (Who can act?)
        bool isManualAction = actionType == WorkflowStepType.Approval ||
                              actionType == WorkflowStepType.Rejection ||
                              actionType == WorkflowStepType.Delegation ||
                              actionType == WorkflowStepType.Recall ||
                              actionType == WorkflowStepType.Escalation;

        if (isManualAction && approverId == null)
        {
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.MissingApprover,
                $"Action type {actionType} requires a valid Approver ID."));
        }

        bool isSystemAction = actionType == WorkflowStepType.Routing ||
                              actionType == WorkflowStepType.AutoApproval ||
                              actionType == WorkflowStepType.PaymentProcessing;
        //comment
        if (isSystemAction && approverId.HasValue)
        {
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.UnexpectedApprover,
                $"Action type {actionType} must not be tied to a specific human Approver ID."));
        }

        // Check 4: Reason/Audit Invariant for Rejecting only
        if (string.IsNullOrEmpty(trimmedReason) && (statusAfter == InvoiceStatus.Rejected || statusAfter == InvoiceStatus.Voided))
        {
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.MissingReason,
               "An audit reason is required for every workflow step."));
        }
        if (trimmedReason.Length > 0 && trimmedReason.Length > MaxReasonlength)
        {
            errors.Add(Error.Validation(WorkflowStepErrors.Creation.ReasonLengthViolation, "Reason must be lesser than 500 characters"));
        }

        // --- 2. Return Failure or Success ---
        if (errors.Count > 0)
        {
            return Result<WorkflowStep>.Failure(errors);
        }

        // 3. If valid, call the private constructor
        var newStep = new WorkflowStep(
            Guid.NewGuid(), // Generate a unique ID for the new step
            invoiceId,
            statusBefore,
            statusAfter,
            actionType,
            approverId,
            trimmedReason,
            timestamp);

        return Result<WorkflowStep>.Success(newStep);
    }
}

