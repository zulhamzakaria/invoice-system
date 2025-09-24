using System.Net;

namespace InvoiceSystem.Domain.Entities;

public class Invoice
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string InvoiceNumber { get; private set; }
    public Company Company { get; private set; }
    public Address BillingAddress { get; private set; }
    public Address ShippingAddress { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public List<InvoiceItem> Items { get; private set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);

    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;
    public Employee CreatedBy { get; private set; }
    public Employee? ApprovedBy { get; private set; }

    private Invoice() { } // EF Core needs this

    public Invoice(string invoiceNumber, Company company, Address billing, Address shipping, DateTime date, Employee createdBy)
    {
        if (string.IsNullOrWhiteSpace(invoiceNumber))
            throw new ArgumentException("Invoice number cannot be empty.");

        InvoiceNumber = invoiceNumber;
        Company = company ?? throw new ArgumentNullException(nameof(company));
        BillingAddress = billing ?? throw new ArgumentNullException(nameof(billing));
        ShippingAddress = shipping ?? throw new ArgumentNullException(nameof(shipping));
        InvoiceDate = date;
        CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));
    }

    public void AddItem(string description, int qty, decimal unitPrice)
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot modify items once submitted.");

        Items.Add(new InvoiceItem(description, qty, unitPrice));
    }

    public void SubmitForApproval()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be submitted.");

        if (!Items.Any())
            throw new InvalidOperationException("Cannot submit an invoice without items.");

        Status = InvoiceStatus.PendingApproval;
    }

    public void Approve(Employee approver, decimal approvalLimit)
    {
        if (Status != InvoiceStatus.PendingApproval)
            throw new InvalidOperationException("Only pending invoices can be approved.");

        if (TotalAmount > approvalLimit)
            throw new InvalidOperationException("Amount exceeds approver's limit.");

        ApprovedBy = approver;
        Status = InvoiceStatus.Approved;
    }

    public void Reject(Employee approver)
    {
        if (Status != InvoiceStatus.PendingApproval)
            throw new InvalidOperationException("Only pending invoices can be rejected.");

        ApprovedBy = approver;
        Status = InvoiceStatus.Rejected;
    }


    public void Void(Employee user)
    {
        if (Status == InvoiceStatus.Approved || Status == InvoiceStatus.Rejected)
        {
            throw new InvalidOperationException('Processed invoices cannot be voided.');
        }
        Status = InvoiceStatus.Voided;
    }
}

public enum InvoiceStatus
{
    Draft,
    PendingApproval,
    Approved,
    Rejected,
    Voided
}
