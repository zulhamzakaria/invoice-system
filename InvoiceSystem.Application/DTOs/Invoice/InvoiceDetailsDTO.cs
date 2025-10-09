using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceDetailsDTO (Guid Id, string InvoiceNo, Company Company, Address BillingAddress, Address ShippingAddress, DateTime InvoiceDate, List<InvoiceItem> InvoiceItems);
