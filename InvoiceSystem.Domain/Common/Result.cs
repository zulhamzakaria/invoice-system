namespace InvoiceSystem.Domain.Common;

public abstract class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    //public Error Error { get; } // Now references the nested Error class
    public IReadOnlyList<Error> Errors { get; }
    public Error Error => Errors.FirstOrDefault() ?? Error.None;
    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        bool hasErrors = errors != null && errors.Any();

        if (isSuccess && hasErrors)
        {
            throw new ArgumentException("A successful result cannot contain errors", nameof(errors));
        }
        if (!isSuccess && !hasErrors)
        {
            throw new ArgumentException("A failed result must contain error(s)", nameof(errors));
        }
        IsSuccess = isSuccess;
        Errors = errors ?? Array.Empty<Error>();
    }

    private sealed class SuccessResult : Result
    {
        public SuccessResult(IReadOnlyList<Error> errors) : base(true, errors) { }
    }

    private sealed class FailureResult: Result
    {
        public FailureResult(IReadOnlyList<Error> errors) : base(false, errors) { }
    }

    public static Result Failure(Error error) => new FailureResult(new List<Error> { error });

    public static Result Success() => new SuccessResult(Array.Empty<Error>());
    public static Result Failure(IReadOnlyList<Error> errors) => new FailureResult(errors);

}

public static class ResultExtensions
{
    public static Result<TOut> Then<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> next)
    {
        return result.IsSuccess ? next(result.Value) : Result<TOut>.Failure(result.Errors);
    }
}