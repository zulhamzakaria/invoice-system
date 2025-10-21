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


    public async Task<Result<EmployeeCreationDTO>> CreateEmployeeAsync(EmployeeCreationDTO employeeCreationDTO)
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
        await _employeeRepository.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<EmployeeSummaryDTO>>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        var dtos = _employeeMapper.ToSummaryDTOs(employees);
        return Result<IReadOnlyList<EmployeeSummaryDTO>>.Success(dtos);
    }

    public async Task<Result<EmployeeDetailsDTO>> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if(employee == null)
        {
            return Result<EmployeeDetailsDTO>.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee exists"));
        }
        return Result<EmployeeDetailsDTO>.Success(_employeeMapper.ToDetailsDTO(employee));
    }

    public Task<Result<EmployeeUpdateDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO employeeUpdateDTO)
    {
        throw new NotImplementedException();
    }
}
