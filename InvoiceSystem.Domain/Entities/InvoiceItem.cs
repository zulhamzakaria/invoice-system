using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class InvoiceItem
{
    private const int MaxDescriptionLength = 500;
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Invoice Invoice { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    private InvoiceItem() { } // For EF Core

    private InvoiceItem(string description, int quantity, decimal unitPrice)
    {
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static Result<InvoiceItem> Create(string description, int quantity, decimal unitPrice)
    {
        string trimmedDescription = description?.Trim() ?? string.Empty;
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(trimmedDescription))
            errors.Add(Error.Validation(InvoiceItemErrors.Creation.MissingDescription, "Invoice Item Description is required"));
        if (!string.IsNullOrWhiteSpace(trimmedDescription) && trimmedDescription.Length > MaxDescriptionLength)
            errors.Add(Error.Validation(InvoiceItemErrors.Creation.DescriptionLengthViolation, "Item Description length cannot be over 500 characters"));
        if (quantity <= 0)
            errors.Add(Error.Validation(InvoiceItemErrors.Creation.NegativeQuantity, "Quantity must be greater than zero"));
        if (unitPrice < 0)
            errors.Add(Error.Validation(InvoiceItemErrors.Creation.NegativePrice, "Unit price cannot be in negative"));

        if (errors.Count > 0)
            return Result<InvoiceItem>.Failure(errors);

        return Result<InvoiceItem>.Success(new InvoiceItem(trimmedDescription, quantity, unitPrice));
    }


}
