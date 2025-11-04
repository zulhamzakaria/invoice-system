using InvoiceSystem.Application.DTOs.Invoice;
using InvoiceSystem.Application.DTOs.InvoiceItem;
using InvoiceSystem.Application.Mappers;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IInvoiceMapper _invoiceMapper;
        public InvoiceService(IEmployeeRepository employeeRepository,
            IInvoiceRepository invoiceRepository,
            ICompanyRepository companyRepository,
            IInvoiceMapper invoiceMapper)
        {
            _employeeRepository = employeeRepository;
            _invoiceRepository = invoiceRepository;
            _companyRepository = companyRepository;
            _invoiceMapper = invoiceMapper;
        }
        public async Task<Result<InvoiceDetailsDTO>> CreateInvoiceAsync(InvoiceCreationDTO creationDTO)
        {
            var employee = await _employeeRepository.GetByIdAsync(creationDTO.CreatedBy);
            if (employee == null || employee is not Clerk)
            {
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvalidEmployeeRole, "Only Clerk can create invoice"));
            }

            var company = await _companyRepository.GetByIdAsync(creationDTO.CompanyId);
            if (company is null)
            {
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(CompanyErrors.Service.CompanyNotFound, "No such company exists"));
            }

            var billingAddress = company.Addresses.FirstOrDefault(a => a.Type == AddressType.Billing);
            var shippingAddress = company.Addresses.FirstOrDefault(a => a.Type == AddressType.Shipping);

            if (billingAddress is null)
            {
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(CompanyErrors.Common.NoBillingAddress, "The Company has no Billing Address registered. Please add one"));
            }

            if (shippingAddress is null)
            {
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(CompanyErrors.Common.NoShippingAddress, "The Company has no Shipping Address registered. Please add one"));
            }

            var newInvoice = Invoice.Create(creationDTO.InvoiceNo, company, billingAddress, shippingAddress, creationDTO.InvoiceDate, employee);

            if (newInvoice.IsFailure)
            {
                return Result<InvoiceDetailsDTO>.Failure(newInvoice.Errors);
            }

            //invoice items
            var invoice = newInvoice.Value;
            foreach (var invoiceItem in invoice.InvoiceItems)
            {
                var invoiceItemResult = invoice.AddItem(invoiceItem.Description, invoiceItem.Quantity, invoiceItem.UnitPrice);
                if (invoiceItemResult.IsFailure)
                {
                    return Result<InvoiceDetailsDTO>.Failure(invoiceItemResult.Errors);
                }
            }

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return Result<InvoiceDetailsDTO>.Success(_invoiceMapper.ToDetailsDTO(newInvoice.Value));

        }

        public async Task<Result<InvoiceItemDTO>> CreateInvoiceItemAsync(Guid invoiceId, InvoiceItemCreationDTO itemDTO)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice is null)
            {
                return Result<InvoiceItemDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, $"No such invoice exists"));
            }
            if (invoice.Status != InvoiceStatus.Draft)
            {
                return Result<InvoiceItemDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvalidStatus, "Only Draft Invoice can be modified"));
            }

            var itemResult = invoice.AddItem(itemDTO.Description, itemDTO.Quantity, itemDTO.UnitPrice);
            if (itemResult.IsFailure)
            {
                return Result<InvoiceItemDTO>.Failure(itemResult.Errors);
            }

            await _invoiceRepository.SaveChangesAsync();
            return Result<InvoiceItemDTO>.Success(InvoiceItemMapper.ToDetailsDTO(itemResult.Value));

        }

        public async Task<Result> DeleteInvoiceItemsAsync(Guid invoiceId, Guid itemId, Employee employee)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if(invoice is null)
            {
                return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice exists"));
            }
            var invoiceItem = invoice.InvoiceItems.FirstOrDefault(it =>  it.Id == itemId);
            if (invoiceItem is null) 
            { 
                return Result.Failure(Error.Validation(InvoiceItemErrors.Common.InvoiceItemNotFound, "No such Invoice Item exists"));
            }
            invoice.DeleteItem(invoiceItem.Id, employee);
            await _invoiceRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<IReadOnlyList<InvoiceSummaryDTO>>> GetAllInvoicesAsync()
        {
            var results = await _invoiceRepository.GetAllAsync();
            return Result<IReadOnlyList<InvoiceSummaryDTO>>.Success(_invoiceMapper.ToSummaryDTO(results));
        }

        public async Task<Result<InvoiceDetailsDTO>> GetInvoiceByIdAsync(Guid invoiceId)
        {
            var result = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (result is null)
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such invoice exists"));
            return Result<InvoiceDetailsDTO>.Success(_invoiceMapper.ToDetailsDTO(result));
        }

        public async Task<Result> UpdateInvoiceAsync(Guid invoiceId, InvoiceUpdateDTO updateDTO, Guid userId)
        {
            var result = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (result is null)
                return Result<InvoiceDetailsDTO>.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such invoice exists"));

            if (updateDTO.InvoiceDate is null)
            {
                return Result.Failure(Error.Validation(InvoiceErrors.Service.InvalidDate, "Invoice Date cannot be empty"));
            }

            //no user id so far
            var updateResult = result.UpdateInvoiceDate(updateDTO.InvoiceDate.Value, userId);

            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            await _invoiceRepository.UpdateAsync(result);
            await _invoiceRepository.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> UpdateInvoiceStatusAsync(Guid invoiceId, InvoiceStatus nextStatus)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if(invoice is null)
            {
                return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice found"));
            }
            invoice.Status = nextStatus.ToString();
            await _invoiceRepository.UpdateAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> VoidInvoiceAsync(Guid invoiceId, Employee employee)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
            if (invoice is null)
            {
                return Result.Failure(Error.Validation(InvoiceErrors.Service.InvoiceNotFound, "No such Invoice exists"));
            }

            invoice.Void(employee);
            await _invoiceRepository.SaveChangesAsync();
            return Result.Success();
        }
    }
}
