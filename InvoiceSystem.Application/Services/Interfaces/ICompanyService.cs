using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface ICompanyService
{
    Task<Result<CompanyDetailsDTO>> CreateCompanyAsync(CompanyCreationDTO dto);
    Task<Result<CompanyDetailsDTO>> UpdateCompanyAsync(CompanyUpdateDTO dto);
    Task<Result<CompanyDetailsDTO>> GetCompanyByIdAsync(Guid id);
    Task<Result<CompanyDetailsDTO>> GetCompanyByRegistrationNumberAsync(string registrationNumber);
    Task<Result<IReadOnlyList<CompanyDetailsDTO>>> GetAllCompaniesAsync();
}
