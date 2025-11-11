using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceCreationDTO(
    [Required] string InvoiceNo,
    [Required] DateTime InvoiceDate,

    [Required] decimal InvoiceAmount,

    [Required] Guid CompanyId,

    [Required] Guid CreatedBy,

    [Required] List<InvoiceCreationDTO> InvoiceItems);
