using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
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

        var billingAddress = Address.Create(
            dto.BillingAddress.Street,
            dto.BillingAddress.City,
            dto.BillingAddress.Zipcode,
            dto.BillingAddress.State,
            dto.BillingAddress.Country,
            Enum.TryParse(dto.BillingAddress.AddressType)
            )

    }

    private Result<TEnum> TryConvertStringToEnum<TEnum>(
        string inputValue, string errorCode, string fieldName, string expectedValues) where TEnum : struct
    {
        if (!Enum.TryParse<TEnum>(inputValue, true, out TEnum result))
        {
            return Result<TEnum>.Failure(Error.Validation("Invalid enum value",
                $"The {fieldName} value '{inputValue}' is not valid. Expected values are: {expectedValues}."));
        }
        return Result<TEnum>.Success(result);
    }

}
