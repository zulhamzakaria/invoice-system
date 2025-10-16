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
    private readonly IAddressMapper _addressMapper;
    public CompanyService(ICompanyRepository companyRepository, ICompanyMappingService companyMappingService, IAddressMapper addressMapper)
    {
        _companyRepository = companyRepository;
        _companyMappingService = companyMappingService;
        _addressMapper = addressMapper;
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
        return Result<CompanyDetailsDTO>.Success(CompanyMapper.ToDetailsDTO(newCompany));

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
            return Result<CompanyDetailsDTO>.Failure(Error.Validation(CompanyErrors.Service., ""));
    }

    public Task<Result<CompanyDetailsDTO>> GetCompanyByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<CompanyDetailsDTO>> GetCompanyByRegistrationNumberAsync(string registrationNumber)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IReadOnlyList<CompanyDetailsDTO>>> GetAllCompaniesAsync()
    {
        throw new NotImplementedException();
    }
}
