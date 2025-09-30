namespace InvoiceSystem.Domain.Common;
public sealed class Error
{
    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static readonly Error None = new Error(string.Empty, string.Empty);
    public static Error Validation(string code, string message) => new Error(code, message);
    public static Error NullValue => Validation("GENERIC.NULL_VALUE", "A required value was null.");
}
