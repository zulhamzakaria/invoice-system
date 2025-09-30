namespace InvoiceSystem.Domain.Common;

public sealed class Result<TValue> : Result
{
    private readonly TValue _value;

    // Guaranteed to be non-null and accessible only on success.
    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Cannot access value of a failed result.");

    private Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<TValue> Success(TValue value) => new Result<TValue>(value, true, Error.None);

    // Delegates to the base failure constructor, passing default for the value.
    public static new Result<TValue> Failure(Error error) => new Result<TValue>(default!, false, error);

    // Allows seamless conversion from the non-generic Result to the generic Result<T>.
    public static Result<TValue> FromResult(Result result)
    {
        return result.IsSuccess
            ? Success(default!) // If successful, returns a Result<T> with the default value
            : Failure(result.Error);
    }
}
