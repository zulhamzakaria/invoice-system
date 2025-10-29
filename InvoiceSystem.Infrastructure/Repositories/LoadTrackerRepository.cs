using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories;

public class LoadTrackerRepository : ILoadTrackerRepository
{
    private readonly AppDbContext _dbContext;
    public LoadTrackerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(LoadTracker loadTracker)
    {
        await _dbContext.AddAsync(loadTracker);
    }

    public async Task<LoadTracker> GetLoadTrackerByApproverIdAsync(Guid id)
    {
        return await _dbContext.LoadTrackers.FirstOrDefaultAsync(lt => lt.ApproverId == id);
    }

    public IQueryable<LoadTracker> GetQueryableLoadTrackers()
    {
        return _dbContext.LoadTrackers.AsQueryable();
    }

    public async Task<int> SaveChangesAsync(CancellationToken token = default)
    {
        return await _dbContext.SaveChangesAsync(token);
    }

    public void UpdateAsync(LoadTracker loadTracker)
    {
        _dbContext.Update(loadTracker);
    }
}
