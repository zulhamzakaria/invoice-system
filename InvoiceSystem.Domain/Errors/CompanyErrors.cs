namespace InvoiceSystem.Domain.Errors;

public static class CompanyErrors
{
    public static class Creation
    {
        public const string MissingName = "COM.MISSING_COMPANY_NAME";
        public const string MissingRegNo = "COM.MISSING_REGISTRATION_NO";
        public const string MissingAddresses = "COM.MISSING_ADDRESSES";
        public const string MissingBillingAddress = "COM.MISSING_BILLING_ADDRESS";
        public const string MissingShippingAddress = "COM.MISSING_SHIPPING_ADDRESS";
        public const string NameLengthViolation = "COM.COMPANY_NAME_LENGTH_INVALID";
        public const string RegistrationNoViolation = "COM.REGISTRATION_NO_LENGTH_INVALID";
    }
    public static class Service
    {
        public const string CompanyExists = "COM.SIMILAR_REGISTRATION_NO_EXISTED";
        public const string InvalidAddressType = "COM.ADDRESS_TYPE_UNDEFINED";
        public const string CompanyNotFound = "COM.COMPANY_ID_INVALID";
        public const string BillingAddressUpdateFailed = "COM.BILLING_ADDRESS_UPDATE_FAILED";
        public const string ShippingAddressUpdateFailed = "COM.SHIPPING_ADDRESS_UPDATE_FAILED";
    }
    public static class Common
    {
        public const string NoBillingAddress = "COM.BILLING_ADDRESS_UNDEFINED";
        public const string NoShippingAddress = "COM.SHIPPING_ADDRESS_UNDEFINED";
    }
}
