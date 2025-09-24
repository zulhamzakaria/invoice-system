namespace InvoiceSystem.Domain.Entities;

public class InvoiceItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal TotalPrice => Quantity * UnitPrice;

    private InvoiceItem() { } // For EF Core

    public InvoiceItem(string description, int quantity, decimal unitPrice)
    {
        if(string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException("Invoice Item Description is required");
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.");

        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
