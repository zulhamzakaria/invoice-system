using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;

public interface ILoadTrackerRepository
{
    Task<LoadTracker> GetLoadTrackerByApproverIdAsync(Guid id);
    List<LoadTracker> GetQueryableLoadTrackers();
    Task AddAsync(LoadTracker loadTracker);
    Task UpdateAsync(LoadTracker loadTracker);
}
