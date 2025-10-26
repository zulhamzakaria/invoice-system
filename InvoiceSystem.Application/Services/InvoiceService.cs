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
        private readonly IAddressMapper _addressMapper;
        private readonly ICompanyMapper _companyMapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly IInvoiceMapper _invoiceMapper;
        public InvoiceService(IEmployeeRepository employeeRepository,
            IInvoiceRepository invoiceRepository,
            IAddressMapper addressMapper,
            ICompanyMapper companyMapper,
            ICompanyRepository companyRepository,
            IInvoiceMapper invoiceMapper)
        {
            _employeeRepository = employeeRepository;
            _invoiceRepository = invoiceRepository;
            _companyRepository = companyRepository;
            _addressMapper = addressMapper;
            _companyMapper = companyMapper;
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
