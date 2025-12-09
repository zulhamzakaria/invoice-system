using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Application.DTOs.WorkflowSteps;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IInvoiceService
{
    public Task<Result<InvoiceDetailsDTO>> CreateInvoiceAsync(InvoiceCreationDTO creationDTO);
    public Task<Result<InvoiceDetailsDTO>> GetInvoiceByIdAsync(Guid invoiceId);
    public Task<Result<IReadOnlyList<InvoiceSummaryDTO>>> GetAllInvoicesAsync();
    public Task<Result<IReadOnlyList<WorkflowstepHistoryDTO>>> GetInvoiceHistoryAsync(Guid invoiceId);
    public Task<Result<IReadOnlyList<InvoiceApproverTaskDTO>>> GetApproverTasksAsync(Guid employeeId);
    public Result<IReadOnlyList<InvoiceApproverTaskDTO>> GetAllApproversTasks();
    public Task<Result<IReadOnlyList<InvoiceClerkTaskDTO>>> GetClerkTasksAsync(Guid employeeId);
    public Task<Result<IReadOnlyList<InvoiceClerkTaskDTO>>> GetAllClerksTasksAsync();

    public Task<Result> UpdateInvoiceAsync(Guid invoiceId, InvoiceUpdateDTO updateDTO, Guid userId);

    public Task<Result> UpdateInvoiceStatusAsync(Guid invoiceId, InvoiceStatus nextStatus);
    public Task<Result> VoidInvoiceAsync(Guid invoiceId, Employee employee);
    public Task<Result> SubmitInvoiceAsync(Guid invoiceId, Employee employee);


    public Task<Result<InvoiceItemDTO>> CreateInvoiceItemAsync(Guid invoiceId, InvoiceItemCreationDTO itemDTO);
    public Task<Result<IReadOnlyList<InvoiceItemDTO>>> GetAllInvoiceItemsAsync(Guid invoiceId);
    public Task<Result> DeleteInvoiceItemAsync(Guid invoiceId, Guid itemId, Guid employeeId);
    public Task<Result> DeleteInvoiceItemsAsync(Guid invoiceId, IEnumerable<Guid> itemIds , Guid employeeId);

}
