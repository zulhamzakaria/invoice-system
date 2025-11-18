using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.Entities;

public class Invoice : AuditableEntity
{

    private const int MinInvoiceNoLength = 1;
    private const int MaxInvoiceNoLength = 50;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string InvoiceNumber { get; private set; }
    public Company Company { get; private set; }
    public Address BillingAddress { get; private set; }
    public Address ShippingAddress { get; private set; }
    public DateTime InvoiceDate { get; private set; }

    private readonly List<InvoiceItem> _items = new();
    public IReadOnlyCollection<InvoiceItem> InvoiceItems => _items.AsReadOnly();
    public decimal TotalAmount => InvoiceItems.Sum(i => i.TotalPrice);

    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Draft;

    public Employee CreatedBy { get; private set; }

    public Employee? ApprovedBy { get; private set; }
    public Guid? ApprovedById { get; private set; }

    private Invoice() { } // EF Core needs this

    public Invoice(string invoiceNumber, Company company, Address billingAddress, Address shippingAddress, DateTime invoiceDate, Employee createdBy)
    {
        InvoiceNumber = invoiceNumber;
        Company = company;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        InvoiceDate = invoiceDate;
        CreatedBy = createdBy;
    }


    public static Result<Invoice> Create(string invoiceNumber, Company company, Address billingAddress, Address shippingAddress, DateTime invoiceDate, Employee createdBy)
    {
        var trimmedInvoiceNo = invoiceNumber?.Trim() ?? string.Empty;
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(trimmedInvoiceNo))
            errors.Add(Error.Validation(InvoiceErrors.Creation.MissingInvoiceNo, "Invoice Number is required."));
        if (trimmedInvoiceNo.Length < MinInvoiceNoLength && trimmedInvoiceNo.Length > MaxInvoiceNoLength)
            errors.Add(Error.Validation(InvoiceErrors.Creation.InvoiceNoLengthViolation, $"Invoice Number length must be between {MinInvoiceNoLength} and {MaxInvoiceNoLength}"));
        if (invoiceDate.Date > DateTime.UtcNow.Date)
            errors.Add(Error.Validation(InvoiceErrors.Creation.DateInFuture, "Invoice Date shouldn't be in future."));
        if (company is null)
            errors.Add(Error.Validation(InvoiceErrors.Creation.MissingCompany, "Company is required"));
        if (createdBy is null)
            errors.Add(Error.Validation(InvoiceErrors.Creation.MissingCreatedBy, "Clerk is required"));
        if (shippingAddress is null)
            errors.Add(Error.Validation(InvoiceErrors.Creation.MissingShippingAddress, "Shippping Address is required"));
        if (billingAddress is null)
            errors.Add(Error.Validation(InvoiceErrors.Creation.MissingBillingAddress, "Billing Address is required"));

        if (errors.Any())
            return Result<Invoice>.Failure(errors);

        var invoice = new Invoice(invoiceNumber, company, billingAddress,
                                 shippingAddress, invoiceDate, createdBy);

        return Result<Invoice>.Success(invoice);
    }

    public Result<InvoiceItem> AddItem(string description, int qty, decimal unitPrice)
    {
        if (Status != InvoiceStatus.Draft)
            return Result<InvoiceItem>.Failure(Error.Validation(InvoiceErrors.InvoiceItems.CannotModifyItems, "Cannot modify items once submitted."));

        var addedItem = InvoiceItem.Create(description, qty, unitPrice);

        if (addedItem.IsFailure)
        {
            return Result<InvoiceItem>.Failure(Error.Validation(InvoiceErrors.Creation.InvalidInvoiceItems, "Invoice item validation failed."));
        }
        addedItem.Value.SetInvoice(this);
        _items.Add(addedItem.Value);
        return Result<InvoiceItem>.Success(addedItem.Value);
    }

    public void SubmitForApproval(Employee employee)
    {
        if (employee is not Clerk)
            throw new DomainException("Only Clerk can void an invoice.", InvoiceErrors.Submission.InvalidEmployeeRole);
        if (Status != InvoiceStatus.Draft)
            throw new DomainException("Only draft invoices can be submitted for approval.", InvoiceErrors.Approval.InvalidStatus);
        if (!InvoiceItems.Any())
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
        UpdatedById = approver.Id;
        UpdatedAt = DateTime.UtcNow;
        Status = InvoiceStatus.Approved;
    }

    public void Reject(Employee employee)
    {
        if (Status != InvoiceStatus.PendingApproval)
            throw new DomainException("Only pending invoices can be rejected.", InvoiceErrors.Approval.InvalidStatus);
        if (employee is null)
            throw new DomainException("An employee must be provided to reject the invoice.", InvoiceErrors.Approval.MissingApprover);

        UpdatedById = employee.Id;
        UpdatedAt = DateTime.UtcNow;
        Status = InvoiceStatus.Rejected;
    }

    public void Void(Employee employee)
    {
        if (employee is not ICanVoidInvoice)
        {
            throw new DomainException("Only Clerk can void an invoice.", InvoiceErrors.Voiding.InvalidRole);
        }

        if (Status == InvoiceStatus.Approved || Status == InvoiceStatus.Rejected)
        {
            throw new DomainException("Processed invoices cannot be voided.", InvoiceErrors.Voiding.Processed);
        }
        UpdatedById = employee.Id;
        UpdatedAt = DateTime.UtcNow;
        Status = InvoiceStatus.Voided;
    }

    public void DeleteItem(Guid itemId, Employee employee)
    {
        if (employee is not Clerk)
        {
            throw new DomainException("Only Clerk can delete an item", InvoiceItemErrors.Deletion.InvalidActor);
        }
        if (Status != InvoiceStatus.Draft)
        {
            throw new DomainException("Cannot modify items after submission", InvoiceItemErrors.Deletion.InvalidStatus);
        }
        var invoiceItem = _items.FirstOrDefault(x => x.Id == itemId);
        if (invoiceItem == null)
        {
            throw new DomainException("Invoice Item not found", InvoiceItemErrors.Common.InvoiceItemNotFound);
        }
        _items.Remove(invoiceItem);
    }

    public void DeleteAllItems(IEnumerable<Guid> itemIds, Employee employee)
    {
        if (employee is not Clerk)
        {
            throw new DomainException("Only Clerk can delete an item", InvoiceItemErrors.Deletion.InvalidActor);
        }
        if (Status != InvoiceStatus.Draft)
        {
            throw new DomainException("Cannot modify items after submission", InvoiceItemErrors.Deletion.InvalidStatus);
        }

        var itemIdsSet = itemIds.ToHashSet();

        _items.RemoveAll(item => itemIdsSet.Contains(item.Id));
    }

    public void UpdateStatus(InvoiceStatus newStatus)
    {
        if (Status == newStatus) return;

        if (Status == InvoiceStatus.Approved || Status == InvoiceStatus.Voided)
        {
            throw new DomainException("Cannot change the Status for approved and voided Invoices ", InvoiceErrors.Workflow.InvalidStatus);
        }

        Status = newStatus;
    }

    public Result UpdateInvoiceDate(DateTime invoiceDate, Guid employeeId)
    {
        if (Status != InvoiceStatus.Draft)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvalidStatus, "Only Draft invoices can be updated"));
        }

        if (invoiceDate == default)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.InvalidDate, "Invoice Date cannot be empty"));
        }

        if (invoiceDate > DateTime.UtcNow)
        {
            return Result.Failure(Error.Validation(InvoiceErrors.Service.AdvancedDate, "Invoice Date cannot be in the future"));
        }

        InvoiceDate = invoiceDate;
        UpdatedById = employeeId;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    //public Result SubmitInvoice(Employee employee)
    //{
    //    if (employee is not Clerk)
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.Submission.InvalidEmployeeRole, "Only Clerk can Submit an invoice"));
    //    }
    //    if (!_items.Any())
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.InvoiceItems.NoInvoiceItem, "No Invoice Item found"));
    //    }

    //    Status = InvoiceStatus.PendingApproval;
    //    return Result.Success();
    //}

    //public Result ApproveInvoice(Employee employee)
    //{
    //    if (employee is not IApprover)
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.Approval.InvalidEmployeeRole, "Only FO/FM can Approve invoices"));
    //    }
    //    if(Status != InvoiceStatus.PendingApproval)
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.Approval.InvalidInvoiceStatus, "Invoice status must be Pending for Approval"));
    //    }
    //    Status = InvoiceStatus.Approved;
    //    return Result.Success();
    //}

    //public Result RejectInvoice(Employee employee)
    //{
    //    if(employee is not FO)
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.Rejection.InvalidEmployeeRole, "Only FO can Reject invoices"));
    //    }
    //    if(Status != InvoiceStatus.PendingApproval)
    //    {
    //        return Result.Failure(Error.Validation(InvoiceErrors.Rejection.InvalidInvoiceStatus, "Invoice status must be Pending for Approval"));
    //    }
    //    Status = InvoiceStatus.Rejected;
    //    return Result.Success();
    //}

}
