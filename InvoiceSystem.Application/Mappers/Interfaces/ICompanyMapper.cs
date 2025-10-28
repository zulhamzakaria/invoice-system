using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers.Interfaces;

public interface ICompanyMapper
{
    CompanyDetailsDTO ToDetailsDTO(Company company);
    CompanySummaryDTO ToSummaryDTO(Company company);
    List<CompanySummaryDTO> MapToSummaryDTOs(IReadOnlyList<Company> companies);
}
