using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.API.Models;

public static class ErrorCodes
{
    public static Error NotFound<T>() => Build<T>("Not_Found", "not found");
    public static Error InvalidStatus<T>() => Build<T>("Undefined_Status", "status is invalid");

    private static string GetPrefix<T>()
    {
        var name = typeof(T).Name;
        if (name.StartsWith("I") && name.Length > 1 && char.IsUpper(name[1]))
        {
            name = name[1..];
        }
        if (name.EndsWith("Service"))
        {
            name = name[..^"Service".Length];
        }
        return name;
    }
    private static Error Build<T>(string key, string messageTemplate)
    {
        var prefix = GetPrefix<T>();
        var message = $"{prefix} {messageTemplate}";
        var code = $"{prefix}.{key}";

        return Error.Validation(code, message);
    }
}
