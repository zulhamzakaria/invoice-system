using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class LoadTrackerService : ILoadTrackerService
{
    private readonly ILoadTrackerRepository _loadTrackerRepository;
    public LoadTrackerService(ILoadTrackerRepository loadTrackerRepository)
    {
        _loadTrackerRepository = loadTrackerRepository;

    }
    public async Task<Result<Employee>> GetNextApproverAsync()
    {
        var trackers = await _loadTrackerRepository.GetAllWithApproverAsync();
        var FOs = trackers
            .Where(t => t.Approver != null && t.Approver.Status == EmployeeStatus.Active && t.Approver is FO)
            .OrderBy(t => t.LastAssignedAt)
            .ToList();
        if (!FOs.Any())
        {
            //return result 
        }
        var next = FOs.First();
        next.MarkAssigned();
        _loadTrackerRepository.Update(next);
        return Result<Employee>.Success(next.Approver);
    }

    public Task RecordAssignmentAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }
}
