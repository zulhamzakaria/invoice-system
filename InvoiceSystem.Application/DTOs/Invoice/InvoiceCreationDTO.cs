using InvoiceSystem.Application.Validation;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceCreationDTO(
    [Required, NotEqual("string")] string InvoiceNo,
    [Required] DateTime InvoiceDate,

    //[Required] decimal InvoiceAmount,

    [Required] Guid CompanyId,

    [Required] Guid CreatedBy
    );
