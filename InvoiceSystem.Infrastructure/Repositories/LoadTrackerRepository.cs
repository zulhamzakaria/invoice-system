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

    Task ILoadTrackerRepository.AddAsync(LoadTracker loadTracker)
    {
        throw new NotImplementedException();
    }

    Task<LoadTracker> ILoadTrackerRepository.GetLoadTrackerByApproverIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    List<LoadTracker> ILoadTrackerRepository.GetQueryableLoadTrackers()
    {
        throw new NotImplementedException();
    }

    Task<int> ILoadTrackerRepository.SaveChangesAsync(CancellationToken token)
    {
        throw new NotImplementedException();
    }

    Task ILoadTrackerRepository.UpdateAsync(LoadTracker loadTracker)
    {
        throw new NotImplementedException();
    }
}
