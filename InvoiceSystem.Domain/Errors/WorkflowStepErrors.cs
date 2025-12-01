namespace InvoiceSystem.Domain.Errors;

public class WorkflowStepErrors
{
    public static class Creation
    {
        public const string InvalidInvoiceId = "WFS.NO_INVOICE_NUMBER_PROVIDED";
        public const string NoStatusChange = "WFS.NO_INVOICE_STATUS_CHANGE";
        public const string MissingApprover = "WFS.NO_APPROVER_PROVIDED";
        public const string UnexpectedApprover = "WFS.MUST_NOT_BE_HUMAN_APPROVER";
        public const string MissingReason = "WFS.NO_REASON_PROVIDED";
        public const string ReasonLengthViolation = "WFS.REASON_LENGTH_INVALID";
    }
    public static class Rejection
    {
        public const string InvalidApprover = "WFS.INVOICE_STATUS_EMPLOYEE_TYPE_MISMATCHED";
    }
}
