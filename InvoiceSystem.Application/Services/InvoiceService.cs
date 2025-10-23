using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;

namespace InvoiceSystem.Application.Services 
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public InvoiceService(IEmployeeRepository employeeRepository, IInvoiceRepository invoiceRepository)
        {
            _employeeRepository = employeeRepository;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<Result<InvoiceDetailsDTO>> CreateInvoiceAsync(InvoiceCreationDTO creationDTO)
        {
            var employee = await _employeeRepository.GetByIdAsync(creationDTO.CreatedBy);
            if(employee == null || employee is not Clerk)
            {
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, ""));
            }
        }

        public Task<Result<InvoiceItemDTO>> CreateInvoiceItemAsync(Guid invoiceId, InvoiceItemCreationDTO itemDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteInvoiceItemsAsync(Guid invoiceId, Guid itemId, EmployeeType employeeType)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IReadOnlyList<InvoiceSummaryDTO>>> GetAllInvoicesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<InvoiceDetailsDTO>> GetInvoiceByIdAsync(Guid invoiceId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateInvoiceAsync(Guid invoiceId, InvoiceUpdateDTO updateDTO)
        {
            throw new NotImplementedException();
        }

        public Task<Result> VoidInvoiceAsync(Guid invoiceId, EmployeeType employeeType)
        {
            throw new NotImplementedException();
        }
    }
}
