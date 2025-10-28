using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.DTOs.Company;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceUpdateDTO(
    //[property: StringLength(50, MinimumLength =1)]
    //string? InvoiceNo,

    [property: DataType(DataType.Date)]
    DateTime? InvoiceDate

    //[property: Range(0.0001, (double)decimal.MaxValue )]
    //decimal? InvoiceAmount,

    //CompanySummaryDTO? Company,
    //AddressDTO? BillingAddress,
    //AddressDTO? ShippingAddress,

    //List<InvoiceCreationDTO>? InvoiceItems
    );

