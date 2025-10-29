using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;

public interface ILoadTrackerRepository
{
    Task<LoadTracker> GetLoadTrackerByApproverIdAsync(Guid id);
    IQueryable<LoadTracker> GetQueryableLoadTrackers();
    Task AddAsync(LoadTracker loadTracker);
    void UpdateAsync(LoadTracker loadTracker);
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
