using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Application.Services.Helpers.EmployeeHelpers;
using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Interfaces;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeMapper _employeeMapper;
    private readonly ILoadTrackerRepository _loadTrackerRepository;

    public EmployeeService(IEmployeeRepository employeeRepository, 
        IEmployeeMapper employeeMapper, 
        ILoadTrackerRepository loadTrackerRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
        _loadTrackerRepository = loadTrackerRepository;
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

        //register into the LoadTracker
        if(employee is IApprover approver)
        {
            var tracker = LoadTracker.Create(employee.Id);
            await _loadTrackerRepository.AddAsync(tracker);
            await _loadTrackerRepository.SaveChangesAsync();
        }

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

    public async Task<Result<IReadOnlyList<Employee>>> GetEmployeesByType(EmployeeType employeeType)
    {
        var results = await _employeeRepository.GetByTypeAsync(employeeType);

        if (results is null || !results.Any())
        {
            return Result<IReadOnlyList<Employee>>.Failure(
                Error.Validation(EmployeeErrors.Service.NoEmployees, $"No Employee Found for type {employeeType.ToString()}"));
        }

        return Result<IReadOnlyList<Employee>>.Success(results);    
    }

    public async Task<Result<EmployeeUpdateDTO>> UpdateEmployeeAsync(Guid id, EmployeeUpdateDTO employeeUpdateDTO)
    {
        var result = await GetEmployeeDetailsByIdAsync(id);
        if (!result.IsSuccess)
        {
            return Result<EmployeeUpdateDTO>.Failure(result.Errors);
        }
        var employee = result.Value;
        var updatedEmployee = employee.PatchEmployee(employeeUpdateDTO.Name, employeeUpdateDTO.Email, employeeUpdateDTO.ApprovalLimit);

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
