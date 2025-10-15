using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface ICompanyService
{
    Task<Result<CompanyDetailsDTO>> CreateCompanyAsync(CompanyCreationDTO dto);
}
