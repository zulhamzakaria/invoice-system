using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;

namespace InvoiceSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    public Task<Result<EmployeeCreationDTO>> CreateEmployeeAsync(EmployeeCreationDTO employeeCreationDTO)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeactivateEmployeeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<EmployeeSummaryDTO>>> GetAllEmployeesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<EmployeeDetailsDTO>> GetEmployeeByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<EmployeeUpdateDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO employeeUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
