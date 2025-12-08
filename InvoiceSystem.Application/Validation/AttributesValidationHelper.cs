using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.Validation;

public static class AttributesValidationHelper
{
    public static IDictionary<string, string[]> ValidateDTO(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, validationContext, results, validateAllProperties: true);

        // flatten all errors per property
        var errorsDict = results
            .SelectMany(r => r.MemberNames.DefaultIfEmpty(string.Empty)
                        .Select(m => new { Member = m, Error = r.ErrorMessage ?? string.Empty }))
            .GroupBy(x => x.Member)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Error).ToArray()
            );

        return errorsDict;
    } 

    public static Result<Dictionary<string, string>> ValidateObjectManually(object dto)
    {
        switch (dto)
        {
            case EmployeeCreationDTO createDto:
                return ValidateFields(createDto.Name, createDto.Email);
            case EmployeeUpdateDTO updateDTO:
                return ValidateFields(updateDTO.Name, updateDTO.Email ?? "");
            default:
                return Result<Dictionary<string, string>>.Success(new Dictionary<string, string>());
        }      
    }

    private static Result<Dictionary<string, string>> ValidateFields(string name, string email)
    {
        if (name == "string" || email == "string")
        {
            return Result<Dictionary<string, string>>.Failure(Error.Validation("DEFAULT_VALUE", "Name/Email cannot be in default values"));
        }
        return Result<Dictionary<string, string>>.Success(new Dictionary<string, string>());
    }
}
