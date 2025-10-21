using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services.Helpers.EmployeeHelpers;
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


    public async Task<Result<EmployeeDetailsDTO>> CreateEmployeeAsync(EmployeeCreationDTO employeeCreationDTO)
    {
        if (await _employeeRepository.EmployeeExists(employeeCreationDTO.Email))
        {
            return Result<EmployeeDetailsDTO>.Failure(Error.Validation(EmployeeErrors.Service.InvalidEmailAddress, "An active Employee with this email already exists"));
        }
        var employeeResult = EmployeeFactory.Create(employeeCreationDTO.Name, employeeCreationDTO.Email, employeeCreationDTO.EmployeeRole, employeeCreationDTO.ApprovalLimit);
        if (employeeResult.IsFailure)
        {
            return Result<EmployeeDetailsDTO>.Failure(employeeResult.Errors);
        }
        var employee = employeeResult.Value;
        await _employeeRepository.AddAsync(employee);
        await _employeeRepository.SaveChangesAsync();
        return Result<EmployeeDetailsDTO>.Success(_employeeMapper.ToDetailsDTO(employee));
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
        var result = await GetEmployeeDetailsByIdAsync(id);
        if (!result.IsSuccess)
        {
            return Result<EmployeeDetailsDTO>.Failure(result.Errors);
        }
        return Result<EmployeeDetailsDTO>.Success(_employeeMapper.ToDetailsDTO(result.Value));
    }

    public async Task<Result<EmployeeUpdateDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO employeeUpdateDTO)
    {
        var result = await GetEmployeeDetailsByIdAsync(id);
        if (!result.IsSuccess)
        {
            return Result<EmployeeUpdateDTO>.Failure(result.Errors);
        }
        var employee = result.Value;
        var updatedEmployee = employee
            .UpdateName(employeeUpdateDTO.Name)
            .Then(e => e.UpdateEmail(employeeUpdateDTO.Email));
        //update email

        if (!updatedEmployee.IsSuccess) {
            return Result<EmployeeUpdateDTO>.Failure(updatedEmployee.Errors);
        }
        await _employeeRepository.UpdateAsync(updatedEmployee.Value);
        await _employeeRepository.SaveChangesAsync();
        return Result<EmployeeUpdateDTO>.Success(employeeUpdateDTO);

    }

    private async Task<Result<Employee>> GetEmployeeDetailsByIdAsync(Guid id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
        {
            return Result<Employee>.Failure(Error.Validation(EmployeeErrors.Service.EmployeeNotFound, "No such Employee exists"));
        }
        return Result<Employee>.Success(employee);
    }

}
