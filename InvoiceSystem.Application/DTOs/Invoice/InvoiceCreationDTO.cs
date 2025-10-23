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
    Guid CompanyId,

    [Required]
    Guid CreatedBy,

    [Required]
    List<InvoiceCreationDTO> InvoiceItems);
