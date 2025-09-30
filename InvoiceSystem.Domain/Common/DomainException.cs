namespace InvoiceSystem.Domain.Common;

public class DomainException : InvalidOperationException
{
    public string? ErrorCode { get; }
    public IReadOnlyDictionary<string, object> DataContext { get; }
    public DomainException(string message, string errorCode, IReadOnlyDictionary<string,object>? dataContext) : base(message) {
        if (string.IsNullOrWhiteSpace(message)) 
            throw new ArgumentNullException(nameof(message));
        if (string.IsNullOrWhiteSpace(errorCode)) 
            throw new ArgumentNullException(nameof(errorCode));
        ErrorCode = errorCode;
        DataContext = dataContext is not null ? 
                        new Dictionary<string, object>(dataContext) : 
                        new Dictionary<string, object>();
    }
    public DomainException(string message, string errorCode) : this(message, errorCode, null) {}
}
