using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.DTOs.Invoice;

public record InvoiceClerkTaskDTO(
    Guid InvoiceId,
    string InvoiceNo,
    InvoiceStatus InvoiceStatus
    );
