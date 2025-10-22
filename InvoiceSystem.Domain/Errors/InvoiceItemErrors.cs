namespace InvoiceSystem.Domain.Errors;

public static class InvoiceItemErrors
{
    public static class Creation
    {
        public const string MissingDescription = "ITM.ITEM_DESCRIPTION_REQUIRED";
        public const string DescriptionLengthViolation = "ITM.DESCRIPTION_LENGTH_INVALID";
        public const string NegativeQuantity = "ITM.QUANTITY_LESSER_THAN_ZERO";
        public const string NegativePrice = "ITM.PRICE_LESSER_THAN_ZERO";
    }
    public static class Deletion
    {
        public const string InvalidActor = "ITM.EMPLOYEE_NOT_A_CLERK";
        public const string InvalidStatus = "ITM.INVOICE_NOT_A_DRAFT";
    }
}
