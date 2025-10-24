using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public class InvoiceMappper : IInvoiceMapper
{
    private readonly ICompanyMapper _companyMapper;
    private readonly IAddressMapper _addressMapper;
    public InvoiceMappper(ICompanyMapper companyMapper, IAddressMapper addressMapper)
    {
        _companyMapper = companyMapper;
        _addressMapper = addressMapper;
    }
    public InvoiceDetailsDTO ToDetailsDTO(Invoice invoice)
    {
        var company = _companyMapper.ToDetailsDTO(invoice.Company);
        var billingAddress = _addressMapper.ToAddressDTO(invoice.BillingAddress);
        var shippingAddress = _addressMapper.ToAddressDTO(invoice.ShippingAddress);

        return new InvoiceDetailsDTO(
            invoice.Id, 
            invoice.InvoiceNumber, 
            invoice.InvoiceDate, 
            invoice.TotalAmount, 
            invoice.Status.ToString(), 
            company, 
            billingAddress, 
            shippingAddress,
            invoice.InvoiceItems.Select(InvoiceItemMapper.ToDetailsDTO).ToList()
            );

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
