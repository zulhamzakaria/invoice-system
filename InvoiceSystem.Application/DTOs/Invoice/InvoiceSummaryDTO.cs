using InvoiceSystem.Application.DTOs.Company;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceSummaryDTO (
    Guid Id,
    string InvoiceNo,
    DateTime InvoiceDate,
    decimal InvoiceAmount,
    string Status,
    CompanySummaryDTO Company,
    int TotalItemsCount);
