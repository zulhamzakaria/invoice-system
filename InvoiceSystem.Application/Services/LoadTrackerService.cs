using InvoiceSystem.Application.Services.Interfaces;
using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using InvoiceSystem.Domain.Errors;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Application.Services;

public class LoadTrackerService : ILoadTrackerService
{
    private readonly ILoadTrackerRepository _loadTrackerRepository;
    public LoadTrackerService(ILoadTrackerRepository loadTrackerRepository)
    {
        _loadTrackerRepository = loadTrackerRepository;

    }
    public async Task<Result<Employee>> GetNextApproverAsync(decimal invoiceAmount)
    {
        var trackers = await _loadTrackerRepository.GetAllWithApproverAsync();
        var FOs = trackers
            .Where(t => t.Approver != null && t.Approver.Status == EmployeeStatus.Active && t.Approver is FO fo && fo.CanApprove(invoiceAmount))
            .OrderBy(t => t.LastAssignedAt)
            .ToList();
        if (!FOs.Any())
        {
            return Result<Employee>.Failure(Error.Validation(LoadTrackerErrors.Service.ApproversNotFound, "No eligible FO(s) found. Please add one"));
        }
        var next = FOs.First();
        //next.MarkAssigned();
        //_loadTrackerRepository.Update(next);
        //await _loadTrackerRepository.SaveChangesAsync();
        return Result<Employee>.Success(next.Approver);
    }

    public async Task<Result> RecordAssignmentAsync(Guid employeeId)
    {
        var FO = await _loadTrackerRepository.GetApproverByIdAsync(employeeId);
        if (FO is null) 
        {
            return Result.Failure(Error.Validation(LoadTrackerErrors.Service.ApproverNotFound, "No such Approver exists"));
        }
        FO.MarkAssigned();
        _loadTrackerRepository.Update(FO);
        await _loadTrackerRepository.SaveChangesAsync();
        return Result.Success();
    }
}
