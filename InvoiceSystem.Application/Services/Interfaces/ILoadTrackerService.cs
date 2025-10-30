using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Application.Services.Interfaces;

public interface ILoadTrackerService
{
    Task<Employee> GetNextApproverAsync();
    Task RecordAssignmentAsync(Guid employeeId);
}
