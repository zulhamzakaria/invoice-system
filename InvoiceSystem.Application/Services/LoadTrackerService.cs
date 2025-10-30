using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class LoadTrackerService : ILoadTrackerService
{
    private readonly ILoadTrackerRepository _loadTrackerRepository;
    public LoadTrackerService(ILoadTrackerRepository loadTrackerRepository)
    {
        _loadTrackerRepository = loadTrackerRepository;
        
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
