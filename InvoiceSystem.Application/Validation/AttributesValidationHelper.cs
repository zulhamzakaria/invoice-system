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
}
