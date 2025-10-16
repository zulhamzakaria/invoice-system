using InvoiceSystem.Application.DTOs.Address;
using InvoiceSystem.Application.DTOs.Company;
using InvoiceSystem.Application.Mappers;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyMappingService _companyMappingService;
    private readonly ICompanyMapper _companyMapper;
    public CompanyService(ICompanyRepository companyRepository, ICompanyMappingService companyMappingService, ICompanyMapper companyMapper)
    {
        _companyRepository = companyRepository;
        _companyMappingService = companyMappingService;
        _companyMapper = companyMapper;
    }
    public async Task<Result<CompanyDetailsDTO>> CreateCompanyAsync(CompanyCreationDTO dto)
    {
        //Check existin company
        var company = await _companyRepository.ExistsByRegistrationNumberAsync(dto.RegistrationNumber);
        if (company)
        {
            var error = new List<Error> { Error.Validation(CompanyErrors.Service.CompanyExists, "A company record with the provided unique identifiers already exists") };
            return Result<CompanyDetailsDTO>.Failure(error);
        }
        var createdBillingAddress = ValidateAndCreateAddress(dto.BillingAddress);
        var creatatedShippingAddress = ValidateAndCreateAddress(dto.ShippingAddress);
        var errors = new List<Error>();
        if (createdBillingAddress.IsFailure) errors.AddRange(createdBillingAddress.Errors);
        if (creatatedShippingAddress.IsFailure) errors.AddRange(creatatedShippingAddress.Errors);
        if (errors.Any())
        {
            return Result<CompanyDetailsDTO>.Failure(errors);
        }
        var createdCompany = Company.Create(dto.Name, dto.RegistrationNumber, createdBillingAddress.Value, creatatedShippingAddress.Value);
        if (createdCompany.IsFailure)
        {
            return Result<CompanyDetailsDTO>.Failure(createdCompany.Errors);
        }
        var newCompany = createdCompany.Value;
        await _companyRepository.AddAsync(newCompany);
        await _companyRepository.SaveChangesAsync();
        return Result<CompanyDetailsDTO>.Success(_companyMapper.ToDetailsDTO(newCompany));

    }


    private Result<Address> ValidateAndCreateAddress(AddressDTO dto)
    {
        var validateEnum = TryConvertStringToEnum<AddressType>(dto.AddressType, CompanyErrors.Service.InvalidAddressType, "Address Type", "Billing or Shipping");
        if (validateEnum.IsFailure)
        {
            return Result<Address>.Failure(validateEnum.Errors);
        }

        return Address.Create(
            dto.Street,
            dto.City,
            dto.State,
            dto.Zipcode,
            dto.Country,
           validateEnum.Value
            );
    }

    private Result<TEnum> TryConvertStringToEnum<TEnum>(string inputValue, string errorCode, string fieldName, string expectedValues)
        where TEnum : struct
    {
        if (!Enum.TryParse<TEnum>(inputValue, true, out TEnum result))
        {
            return Result<TEnum>.Failure(Error.Validation("Invalid enum value",
                $"The {fieldName} value '{inputValue}' is not valid. Expected values are: {expectedValues}."));
        }
        return Result<TEnum>.Success(result);
    }

    public async Task<Result<CompanyDetailsDTO>> UpdateCompanyAsync(Guid id, CompanyUpdateDTO dto)
    {
        var result = await _companyRepository.GetByIdAsync(id);
        if(result is null)
        {
            return Result<CompanyDetailsDTO>.Failure(Error.Validation(CompanyErrors.Service.CompanyNotFound, "No Such Company Exists"));
        }
            

        var mergeResult = _companyMappingService.MergeAndValidateAddress(result, dto);
        if (mergeResult.IsFailure)
        {
            return Result<CompanyDetailsDTO>.Failure(mergeResult.Errors);
        }

        var (newBillingAddress, newShippingAddress) = mergeResult.Value;

        //var updateResult = result.

        //if (updateResult.IsFailure)
        //{
        //    return Result<CompanyDetailsDTO>.Failure(updateResult.Errors);
        //}

        await _companyRepository.SaveChangesAsync();
        return Result<CompanyDetailsDTO>.Success(_companyMapper.ToDetailsDTO(result));


    }

    public async Task<Result<CompanyDetailsDTO>> GetCompanyByIdAsync(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id);
        if(company is null)
        {
            return Result<CompanyDetailsDTO>.Failure(Error.Validation(CompanyErrors.Service.CompanyNotFound, "No Such Company Exists"));
        }
        var dto = _companyMapper.ToDetailsDTO(company);
        return Result<CompanyDetailsDTO>.Success(dto);
    }

    public async Task<Result<CompanyDetailsDTO>> GetCompanyByRegistrationNumberAsync(string registrationNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<CompanySummaryDTO>>> GetAllCompaniesAsync()
    {
        var companies = await _companyRepository.GetAllAsync();
        var dtos = _companyMapper.MapToSummaryDTOs(companies);
        return Result<List<CompanySummaryDTO>>.Success(dtos);
    }
}
