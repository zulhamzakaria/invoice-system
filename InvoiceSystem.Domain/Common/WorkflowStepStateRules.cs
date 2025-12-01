using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.Common;

public static class WorkflowStepStateRules
{
    public static InvoiceStatus DetermineNextStatus(InvoiceStatus currentStatus, WorkflowStepType stepType, EmployeeType? employeeType)
    {
        return (currentStatus, stepType, employeeType) switch
        {
            //(InvoiceStatus.Draft, WorkflowStepType.Submission, EmployeeType.Clerk) => InvoiceStatus.PendingOfficerApproval,

            //(InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Approval, EmployeeType.FO) => InvoiceStatus.PendingManagerApproval,
            //(InvoiceStatus.PendingManagerApproval, WorkflowStepType.Approval, EmployeeType.FM) => InvoiceStatus.ApprovedByManager,
            //(InvoiceStatus.PendingOfficerApproval, WorkflowStepType.AutoApproval, _) => InvoiceStatus.ApprovedByManager,

            // REJECTIONS — only valid at approval stages
            (InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Rejection, EmployeeType.FO) => InvoiceStatus.Rejected,
            (InvoiceStatus.PendingManagerApproval, WorkflowStepType.Rejection, EmployeeType.FM) => InvoiceStatus.Rejected,

            ////VOID
            //(InvoiceStatus.Draft, WorkflowStepType.Void, EmployeeType.Clerk) => InvoiceStatus.Voided,
            //(InvoiceStatus.Rejected, WorkflowStepType.Void, EmployeeType.Clerk) => InvoiceStatus.Voided,

            //(InvoiceStatus.PendingOfficerApproval, WorkflowStepType.Routing, _) => InvoiceStatus.PendingOfficerApproval,
            //(InvoiceStatus.ApprovedByManager, WorkflowStepType.PaymentProcessing, _) => InvoiceStatus.Paid,

            //(_, WorkflowStepType.Recall, _) => InvoiceStatus.Draft,
            //(_, WorkflowStepType.Delegation, _) => currentStatus,
            //(_, WorkflowStepType.Escalation, _) => currentStatus,

            _ => currentStatus
        };
    }
}
