using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.API.Models;

public sealed record ErrorResponse
{
    public string Title { get; init; } = "Bad request";
    public int Status {  get; init; } = 400;
    public IReadOnlyList<ErrorDetail> Errors { get; init; }

    public ErrorResponse(IReadOnlyList<Error> errors)
    {
        Errors = errors.Select(e => new ErrorDetail(e.Code, e.Message)).ToList();
    }
}

public sealed record ErrorDetail(string Code, string Message);
