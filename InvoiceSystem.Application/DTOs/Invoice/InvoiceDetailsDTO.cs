using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.DTOs.InvoiceItem;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceDetailsDTO(
    Guid Id,
    string InvoiceNo,
    DateTime InvoiceDate,

    decimal InvoiceAmount,
    string Status, 
                                
    CompanySummaryDTO Company,
    AddressDTO BillingAddress,
    AddressDTO ShippingAddress,

    List<InvoiceItemDTO> InvoiceItems);
