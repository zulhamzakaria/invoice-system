using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceCreationDTO(
    [property: Required, StringLength(50, MinimumLength =1)]
    string InvoiceNo,
    [property: Required, DataType(DataType.Date)]
    DateTime InvoiceDate,

    [property: Required, Range(0.0001, (double)decimal.MaxValue )]
    decimal InvoiceAmount,

    [Required]
    CompanySummaryDTO Company,
    [Required]
    AddressDTO BillingAddress,
    [Required]
    AddressDTO ShippingAddress,

    [Required]
    List<InvoiceCreationDTO> InvoiceItems);
