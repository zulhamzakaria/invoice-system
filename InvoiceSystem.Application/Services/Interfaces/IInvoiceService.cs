using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IInvoiceService
{
    public Task<Result<InvoiceDetailsDTO>> CreateInvoiceAsync(InvoiceCreationDTO creationDTO);
    public Task<Result<InvoiceDetailsDTO>> GetInvoiceByIdAsync(Guid invoiceId);
    public Task<Result<IReadOnlyList<InvoiceSummaryDTO>>> GetAllInvoicesAsync();

    public Task<Result> UpdateInvoiceAsync(Guid invoiceId, InvoiceUpdateDTO updateDTO, Guid userId);

    public Task<Result> UpdateInvoiceStatusAsync(Guid invoiceId, InvoiceStatus nextStatus);
    public Task<Result> VoidInvoiceAsync(Guid invoiceId, Employee employee);


    public Task<Result<InvoiceItemDTO>> CreateInvoiceItemAsync(Guid invoiceId, InvoiceItemCreationDTO itemDTO);

    public Task<Result<IReadOnlyList<InvoiceItemDTO>>> GetAllInvoiceItemsAsync(Guid invoiceId);
    public Task<Result> DeleteInvoiceItemsAsync(Guid invoiceId, Guid itemId, Employee employee);
 }
