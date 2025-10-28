using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IInvoiceService
{
    public Task<Result<InvoiceDetailsDTO>> CreateInvoiceAsync(InvoiceCreationDTO creationDTO);
    public Task<Result<InvoiceDetailsDTO>> GetInvoiceByIdAsync(Guid invoiceId);
    public Task<Result<IReadOnlyList<InvoiceSummaryDTO>>> GetAllInvoicesAsync();

    public Task<Result> UpdateInvoiceAsync(Guid invoiceId, InvoiceUpdateDTO updateDTO, Guid userId);
    public Task<Result> VoidInvoiceAsync(Guid invoiceId, EmployeeType employeeType);


    public Task<Result<InvoiceItemDTO>> CreateInvoiceItemAsync(Guid invoiceId, InvoiceItemCreationDTO itemDTO);
    public Task<Result> DeleteInvoiceItemsAsync(Guid invoiceId, Guid itemId, EmployeeType employeeType);
 }
