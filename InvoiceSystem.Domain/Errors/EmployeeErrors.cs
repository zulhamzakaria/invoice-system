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

    public static class Updating
    {
        public const string InvalidApprover = "EMP.EMPLOYEE_NOT_AN_APPROVER";
        public const string InvalidApprovalAmount = "EMP.APPROVAL_AMOUNT_IN_NEGATIVE";
    }
    public static class Service
    {
        public const string EmployeeNotFound = "EMP.EMPLOYEE_ID_INVALID";
        public const string InvalidEmployeeType = "EMP.EMPLOYEE_TYPE_UNDEFINED";
        public const string InvalidFOApprovalLimit = "EMP.FO_APPROVAL_LIMIT_UNDEFINED";
        public const string InvalidEmailAddress = "EMP.EMPLOYEE_EMAIL_IN_USED";
        public const string ApprovalLimitExceeded = "EMP.FO_APPROVAL_LIMIT_NOT_ENOUGH";
        public const string InvalidApprover = "EMP.EMPLOYEE_NOT_AN_APPROVER";
        public const string NoEmployees = "EMP.NO_EMPLOYEE_REGISTERED";
     }
}
