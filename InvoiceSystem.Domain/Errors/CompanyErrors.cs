namespace InvoiceSystem.Domain.Errors;

public static class CompanyErrors
{
    public static class Creation
    {
        public const string MissingName = "COM.MISSING_COMPANY_NAME";
        public const string MissingRegNo = "COM.MISSING_REGISTRATION_NO";
        public const string MissingBillingAddress = "COM.MISSING_BILLING_ADDRESS";
        public const string MissingShippingAddress = "COM.MISSING_SHIPPING_ADDRESS";
    }
}
