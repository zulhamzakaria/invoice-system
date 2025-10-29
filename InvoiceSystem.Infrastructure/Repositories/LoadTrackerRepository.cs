using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;

namespace InvoiceSystem.Infrastructure.Repositories;

public class LoadTrackerRepository : ILoadTrackerRepository
{
    public Task AddAsync(LoadTracker loadTracker)
    {
        throw new NotImplementedException();
    }

    public Task<LoadTracker> GetLoadTrackerByApproverIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<LoadTracker> GetQueryableLoadTrackers()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(LoadTracker loadTracker)
    {
        throw new NotImplementedException();
    }
}
