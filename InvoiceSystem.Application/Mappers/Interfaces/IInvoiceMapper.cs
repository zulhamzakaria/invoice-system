using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers.Interfaces;

public interface IInvoiceMapper
{
    InvoiceDetailsDTO ToDetailsDTO(Invoice invoice);
    InvoiceSummaryDTO ToSummaryDTO(Invoice invoice);
    IReadOnlyList<InvoiceSummaryDTO> ToSummaryDTO(IEnumerable<Invoice> invoices);
}
