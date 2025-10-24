using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public class InvoiceMappper : IInvoiceMapper
{
    public InvoiceDetailsDTO ToDetailsDTO(Invoice invoice)
    {
        throw new DomainException("nothing here","cock");
    }

    public InvoiceSummaryDTO ToSummaryDTO(Invoice invoice)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<InvoiceDetailsDTO> ToSummaryDTO(IEnumerable<Invoice> invoices)
    {
        throw new NotImplementedException();
    }
}
