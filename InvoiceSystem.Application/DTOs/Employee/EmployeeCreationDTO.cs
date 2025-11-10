using InvoiceSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSystem.Application.DTOs.Employee;

public record EmployeeCreationDTO(
    [Required, StringLength(100,MinimumLength =1)]
    string Name,
    [Required, EmailAddress, StringLength(100,MinimumLength =1)]
    string Email,
    ///summary
    ///Valid EmployeeType: Clerk, FO, FM
    /// </summary>
    [Required]
    EmployeeType EmployeeRole,
    [Range(0.0001, (double)decimal.MaxValue)]
    decimal? ApprovalLimit
    );
