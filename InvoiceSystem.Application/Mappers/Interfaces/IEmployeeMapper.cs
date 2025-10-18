using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers.Interfaces;

public interface IEmployeeMapper
{
    EmployeeDetailsDTO ToDetailsDTO(Employee employee);
    IReadOnlyList<EmployeeSummaryDTO> ToSummaryDTOs(IReadOnlyList<Employee> employees);
}
