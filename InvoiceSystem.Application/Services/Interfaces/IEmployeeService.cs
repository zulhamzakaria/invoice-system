using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<Result<EmployeeDetailsDTO>> GetEmployeeByIdAsync(Guid id);
    Task<Result<IReadOnlyList<EmployeeSummaryDTO>>> GetAllEmployeesAsync();
    Task<Result<EmployeeDetailsDTO>> CreateEmployeeAsync(EmployeeCreationDTO employeeCreationDTO);
    Task<Result<EmployeeUpdateDTO>> UpdateEmployeeAsync(Guid id,  EmployeeUpdateDTO employeeUpdateDTO);
    Task<Result> DeactivateEmployeeAsync(Guid id);
    Task<Result<IReadOnlyList<Employee>>> GetEmployeesByType(EmployeeType employeeType); 

}
