using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;

public interface ILoadTrackerRepository
{
    Task<LoadTracker> GetApproverByIdAsync(Guid id);
    IQueryable<LoadTracker> Query();
    Task AddAsync(LoadTracker loadTracker);
    void Update(LoadTracker loadTracker);
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
