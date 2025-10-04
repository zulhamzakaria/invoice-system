using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Domain.Entities;

public class Invoice:AuditableEntity
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

    public Invoice(string invoiceNumber, Company company, Address billingAddress, Address shippingAddress, DateTime invoiceDate, Employee createdBy)
    {
        InvoiceNumber = invoiceNumber;
        Company = company ;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        InvoiceDate = invoiceDate;
        CreatedBy = createdBy;
    }


    public static Result<Invoice> Create(string invoiceNumber, Company company, Address billingAddress, Address shippingAddress, DateTime invoiceDate, Employee createdBy)
    {
        if (string.IsNullOrEmpty(invoiceNumber))
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.MissingInvoiceNo,"Invoice Number is required."));
        if (invoiceDate.Date > DateTime.UtcNow.Date)
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.DateInFuture, "Invoice Date shouldn't be in future."));
        if (company is null)
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.MissingCompany, "Company is required"));
        if (createdBy is null)
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.MissingCreatedBy, "Clerk is required"));
        if (shippingAddress is null)
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.MissingShippingAddress, "Shippping Address is required"));
         if (billingAddress is null)
            return Result<Invoice>.Failure(Error.Validation(InvoiceErrors.Creation.MissingBillingAddress, "Billing Address is required"));

        var invoice = new Invoice(invoiceNumber, company, billingAddress,
                                 shippingAddress, invoiceDate, createdBy);

        return Result<Invoice>.Success(invoice);
    }

    public void AddItem(string description, int qty, decimal unitPrice)
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Cannot modify items once submitted.", InvoiceErrors.InvoiceItems.CannotModifyItems);

        Items.Add(new InvoiceItem(description, qty, unitPrice));
    }

    public void SubmitForApproval()
    {
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Only draft invoices can be submitted for approval.", InvoiceErrors.Approval.InvalidStatus);

        if (!Items.Any())
            throw new DomainException("Cannot submit an invoice without items.", InvoiceErrors.InvoiceItems.NoInvoiceItem);

        Status = InvoiceStatus.PendingApproval;
    }

    public void Approve(Employee approver, decimal approvalLimit)
    {
        if (Status != InvoiceStatus.PendingApproval)
            throw new DomainException("Only pending invoices can be approved.", InvoiceErrors.Approval.InvalidStatus);

        if (approver is null)
            throw new DomainException("An approver must be provided to approve the invoice.", InvoiceErrors.Approval.MissingApprover);

        if (approver is not IApprover approverWithLimit || !approverWithLimit.CanApprove(TotalAmount))
            throw new DomainException("Approver cannot approve this invoice.", InvoiceErrors.Approval.LimitExceeded);

        ApprovedBy = approver;
        Status = InvoiceStatus.Approved;
    }

    public void Reject(Employee approver)
    {
        if (Status != InvoiceStatus.PendingApproval)
            throw new DomainException("Only pending invoices can be rejected.", InvoiceErrors.Approval.InvalidStatus);
        if(approver is null)
            throw new DomainException("An approver must be provided to reject the invoice.", InvoiceErrors.Approval.MissingApprover);

        ApprovedBy = approver;
        Status = InvoiceStatus.Rejected;
    }


    public void Void(Employee user)
    {
        if(user is not ICanVoidInvoice)
        {
            throw new DomainException("Only Clerk can void an invoice.", InvoiceErrors.Voiding.InvalidRole);
        }

        if (Status == InvoiceStatus.Approved || Status == InvoiceStatus.Rejected)
        {
            throw new DomainException("Processed invoices cannot be voided.", InvoiceErrors.Voiding.Processed);
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
