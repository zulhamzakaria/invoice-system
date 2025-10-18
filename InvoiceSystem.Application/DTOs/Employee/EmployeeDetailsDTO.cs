namespace InvoiceSystem.Application.DTOs.Employee;

public record EmployeeDetailsDTO(
    Guid Id, 
    string Name, 
    string Email, 
    string EmployeeRole,

    bool IsApprover,
    bool IsLimitlessApprover,
    decimal MaxApprovalAmount
    );
