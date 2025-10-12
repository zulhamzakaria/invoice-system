using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Employee;

public record EmployeeCreationDTO(
    [property: Required, StringLength(100,MinimumLength =1)]
    string Name,
    [property: Required, EmailAddress, StringLength(100,MinimumLength =1)]
    string Email,
    ///summary
    ///Valid EmployeeType: Clerk, FO, FM
    /// </summary>
    [property:Required]
    EmployeeType EmployeeRole
    );
