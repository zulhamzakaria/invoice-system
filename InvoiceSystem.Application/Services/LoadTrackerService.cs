using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class LoadTrackerService : ILoadTrackerService
{
    private readonly ILoadTrackerRepository _loadTrackerRepository;
    private readonly IEmployeeRepository _employeeRepository;
    public LoadTrackerService(ILoadTrackerRepository loadTrackerRepository, IEmployeeRepository employeeRepository)
    {
        _loadTrackerRepository = loadTrackerRepository;
        _employeeRepository = employeeRepository;
    }
    public Task<Employee> GetNextApproverAsync()
    {
        throw new NotImplementedException();
    }

    public Task RecordAssignmentAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }
}
