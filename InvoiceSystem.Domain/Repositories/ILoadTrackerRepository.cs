using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;

public interface ILoadTrackerRepository
{
    Task<LoadTracker?> GetApproverByIdAsync(Guid id);

    Task<List<LoadTracker>> GetAllWithApproverAsync();
    IQueryable<LoadTracker> Query();
    Task AddAsync(LoadTracker loadTracker);
    void Update(LoadTracker loadTracker);
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
