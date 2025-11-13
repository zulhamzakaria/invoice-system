using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

public class EmployeeMapper : IEmployeeMapper
{
    public EmployeeDetailsDTO ToDetailsDTO(Employee employee)
    {

        (string empRole, decimal maxApprovalAmount, bool isApprover, bool isLimitlessApprover) = employee switch
        {
            FM fm => ("FinanceManager", fm.MaxApprovalAmount, true, true),
            FO fo => ("FinanceOfficer", fo.MaxApprovalAmount, true, false),
            Clerk => ("Clerk", 0, false, false),
            _ => throw new InvalidOperationException($"Employee type {employee.GetType().Name} is not defined")
        };

        return new EmployeeDetailsDTO(
            employee.Id,
            employee.Name,
            employee.Email,
            empRole,
            isApprover,
            isLimitlessApprover,
            maxApprovalAmount
            );
    }

    public IReadOnlyList<EmployeeSummaryDTO> ToSummaryDTOs(IReadOnlyList<Employee> employees)
    {
        var summaryDTOs = employees.Select(e => new EmployeeSummaryDTO(
            e.Id,
            e.Name,
            e switch { FM => "Finance Manager", FO => "Finance Officer", _ => "Clerk" }, 
            e.Email))
            .ToList();
        return summaryDTOs;
    }
}
