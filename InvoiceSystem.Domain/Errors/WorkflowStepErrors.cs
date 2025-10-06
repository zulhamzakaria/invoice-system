namespace InvoiceSystem.Domain.Errors;

public class WorkflowStepErrors
{
    public static class Creation
    {
        public const string InvalidInvoiceId = "WFS.NO_INVOCIE_NO_PROVIDED";
        public const string NoStatusChange = "WFS.NO_INVOICE_STATUS_CHANGE";
        public const string MissingApprover = "WFS.NO_APPROVER_PROVIDED";
        public const string UnexpectedApprover = "WFS.MUST_NOT_BE_HUMAN_APPROVER";
        public const string MissingReason = "WFS.NO_REASON_PROVIDED";
    }
}
