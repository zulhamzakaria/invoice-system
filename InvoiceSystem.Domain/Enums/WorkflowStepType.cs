namespace InvoiceSystem.Domain.Enums;

public enum WorkflowStepType
{
    Submission,        // Initial action by the requester

    Routing,           // System action: Determining the next approver (Round Robin)
    AutoApproval,      // System action: Approved below a threshold
    PaymentProcessing, // System action: Triggered for payment

    Approval,          // Manual action: Approver accepted
    Rejection,         // Manual action: Approver denied
    Escalation,        // Manual action: Approver bumped it to a higher level
    Delegation,        // Manual action: Approver delegated to someone else

    Recall,            // Administrative: Requester pulled the request back
    Void,              // Administrative: Cancelled by an admin or system process
}
