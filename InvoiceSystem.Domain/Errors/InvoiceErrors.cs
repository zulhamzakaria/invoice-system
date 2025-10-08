namespace InvoiceSystem.Domain.Errors;

public static class InvoiceErrors
{
    public static class Creation
    {
        public const string MissingInvoiceNo = "INV.MISSING_INVOICE_NO";
        public const string MissingCompany = "INV.MISSING_COMPANY";
        public const string MissingBillingAddress = "INV.MISSING_BILLING_ADDRESS";
        public const string MissingShippingAddress = "INV.MISSING_SHIPPING_ADDRESS";
        public const string MissingCreatedBy = "INV.MISSING_CREATED_BY";
        public const string DateInFuture = "INV.DATE_IN_FUTURE";
        public const string InvalidInvoiceItems = "INV.ITEM_VALIDATION_FAILED";
        public const string InvoiceNoLengthViolation = "INV.INVOICE_NO_LENGTH_INVALID";      
    }
    public static class Voiding
    {
        public const string Processed = "INV.INVOICE_PROCESSED_ALREADY";
        public const string Voided = "INV.INVOICE_VOIDED_ALREADY";
        public const string InvalidRole = "INV.INVALID_ROLE";
    }
    public static class Approval
    {
        public const string InvalidStatus = "INV.INVALID_STATUS"; 
        public const string LimitExceeded = "INV.APPROVAL_LIMIT_EXCEEDED";
        public const string MissingApprover = "INV.NO_APPROVER_PROVIDED";
    }
    public static class  InvoiceItems
    {
        public const string NoInvoiceItem = "INV.NO_INVOICE_ITEMS";
        public const string CannotModifyItems = "INV.CANNOT_MODIFY_ITEMS"
;    }
}
