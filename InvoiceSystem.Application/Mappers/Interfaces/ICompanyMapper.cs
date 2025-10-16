using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers.Interfaces;

public interface ICompanyMapper
{
    CompanyDetailsDTO ToDetailsDTO(Company company);
}
