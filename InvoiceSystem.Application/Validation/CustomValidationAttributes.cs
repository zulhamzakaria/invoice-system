using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.Validation;

//public class CustomValidationAttributes : ValidationAttribute
//{

//}

public class NotEqualAttribute : ValidationAttribute
{
    private readonly string _forbiddenValue;
    public NotEqualAttribute(string value)
    {
        _forbiddenValue = value;
    }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value?.ToString() == _forbiddenValue)
        {
            return new ValidationResult(ErrorMessage);
        }
        return ValidationResult.Success;
    }
}
