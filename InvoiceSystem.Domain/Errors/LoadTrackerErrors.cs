namespace InvoiceSystem.Domain.Errors;

public static class LoadTrackerErrors
{
    public static class Service
    {
        public const string ApproversNotFound = "LTR.NO_ELIGIBLE_APPROVER";
        public const string ApproverNotFound = "LTR.APPROVER_ID_NOT_VALID";
    }
}
