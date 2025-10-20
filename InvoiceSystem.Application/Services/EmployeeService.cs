using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Errors;
using System.Runtime.InteropServices;

namespace InvoiceSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeMapper _employeeMapper;

    public EmployeeService(IEmployeeRepository employeeRepository, IEmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
    }


    public Task<Result<EmployeeCreationDTO>> CreateEmployeeAsync(EmployeeCreationDTO employeeCreationDTO)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeactivateEmployeeAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return Result.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee exists"));
        }
        employee.Deactivate();
        return Result.Success();
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
