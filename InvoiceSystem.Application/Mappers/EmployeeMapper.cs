using InvoiceSystem.Application.DTOs.Employee;
using InvoiceSystem.Application.Mappers.Interfaces;
using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Mappers;

internal class EmployeeMapper : IEmployeeMapper
{
    public EmployeeDetailsDTO ToDetailsDTO(Employee employee)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<EmployeeSummaryDTO> ToSummaryDTOs(IReadOnlyList<Employee> employees)
    {
        throw new NotImplementedException();
    }
}
