namespace InvoiceSystem.Application.DTOs.Employee;

public record EmployeeSummaryDTO(
    Guid Id,
    string Name,
    string EmployeeRole,
    string Email
    );

