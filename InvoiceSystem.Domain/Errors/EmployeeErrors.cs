namespace InvoiceSystem.Domain.Errors;

public static class EmployeeErrors
{
    public static class Creation
    {
        public const string MissingName = "EMP.MISSING_EMPLOYEE_NAME";
        public const string MissingEmail = "EMP.MISSING_EMPLOYEE_EMAIL";
        public const string NameLengthViolation = "EMP.EMPLOYEE_NAME_LENGTH_INVALID";
        public const string EmailLengthViolation = "EMP.EMPLOYEE_EMAIL_LENGTH_INVALID";
        public const string NegativeFOApprovalLimit = "EMP.NEGATIVE_FO_APPROVAL_LIMIT";
    }
    public static class Service
    {
        public const string EmployeeNotFound = "EMP.EMPLOYEE_ID_INVALID";
    }
}
