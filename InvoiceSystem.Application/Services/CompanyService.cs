using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Repositories;
using System.Runtime.CompilerServices;

namespace InvoiceSystem.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    public async Task<Result<CompanyDetailsDTO>> CreateCompanyAsync(CompanyCreationDTO dto)
    {
        //Check existin company
        var company = await _companyRepository.ExistsByRegistrationNumberAsync(dto.RegistrationNumber);
        if (company)
        {
            var errors = new List<Error> { Error.Validation(CompanyErrors.Service.CompanyExists, "A company record with the provided unique identifiers already exists") };
        }

    }
}
