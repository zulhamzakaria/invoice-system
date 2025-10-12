using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Employee;

public record EmployeeUpdateDTO(
    [property: StringLength(100,MinimumLength =1)]
    string Name,
    [property: EmailAddress, StringLength(100,MinimumLength =1)]
    string Email,
    [property: Range(0.0001, (double)decimal.MaxValue)]
    decimal? ApprovalLimit
    );
