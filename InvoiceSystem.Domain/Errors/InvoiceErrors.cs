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
    public static class Workflow
    {
        public const string InvalidStatus = "INV.INVOICE_VOIDED_OR_PROCESSED";
    }
    public static class Voiding
    {
        public const string Processed = "INV.INVOICE_PROCESSED_ALREADY";
        public const string Voided = "INV.INVOICE_VOIDED_ALREADY";
        public const string InvalidEmployeeRole = "INV.EMPLOYEE_NOT_A_CLERK";
        public const string CannotVoid = "INV.NOT_INVOICE_CREATOR";
    }
    public static class Submission
    {
        public const string InvalidEmployeeRole = "INV.EMPLOYEE_NOT_A_CLERK";
    }

    public static class Approval
    {
        public const string InvalidStatus = "INV.INVALID_STATUS"; 
        public const string LimitExceeded = "INV.APPROVAL_LIMIT_EXCEEDED";
        public const string MissingApprover = "INV.NO_APPROVER_PROVIDED";
        public const string InvalidEmployeeRole = "INV.EMPLOYEE_NOT_AN_APPROVER";
        public const string InvalidInvoiceStatus = "INV.INVOICE_STATUS_NOT_PENDING_APPROVAL";
    }
    public static class Rejection
    {
        public const string InvalidEmployeeRole = "INV.EMPLOYEE_NOT_FO";
        public const string InvalidInvoiceStatus = "INV.INVOICE_STATUS_NOT_PENDING_APPROVAL";
    }
    public static class  InvoiceItems
    {
        public const string NoInvoiceItem = "INV.NO_INVOICE_ITEMS";
        public const string CannotModifyItems = "INV.CANNOT_MODIFY_ITEMS";
        public const string ItemIdsMismatched = "INV.PROVIDED_ITEM_IDS_INVALID";
    }
    public static class Service
    {
        public const string InvoiceNotFound = "INV.NO_INVOICE_FOUND";
        public const string InvalidEmployeeRole = "INV.EMPLOYEE_NOT_A_CLERK";
        public const string InvalidStatus = "INV.INVOICE_IS_NOT_DRAFT";
        public const string InvalidDate = "INV.INVOICE_DATE_EMPTY";
        public const string AdvancedDate = "INV.INVOICE_DATE_IN_FUTURE";
        public const string NoAssignedInvoice = "INV.NO_INVOICES_UNDER_EMPLOYEE";
        public const string NotAssignedInvoice = "INV.APPROVER_CANNOT_ACT";
        public const string CannotVoid = "INV.NOT_INVOICE_CREATOR";
        public const string InvoiceExists = "INV.ACTIVE_DUPLICATE_FOUND";
        public const string NoClerksTasks = "INV.NO_CLERKS_TASKS";
    }
}
