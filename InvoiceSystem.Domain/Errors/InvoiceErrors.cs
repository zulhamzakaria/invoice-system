namespace InvoiceSystem.Domain.Errors;

public static class InvoiceErrors
{
    public static class Voiding
    {
        public const string Processed = "INV.VOID_PROCESSED";
        public const string Voided = "INV.VOID_ALREADY";
    }
    public static class Approval
    {
        public const string LimitExceeded = "INV.APPROVAL_LIMIT_EXCEEDED";
    }
}
