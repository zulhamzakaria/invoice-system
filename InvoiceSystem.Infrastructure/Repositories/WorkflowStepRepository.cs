using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories;

public class WorkflowStepRepository : IWorkflowStepRepository
{
    private readonly AppDbContext _context;

    public WorkflowStepRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(WorkflowStep workflowStep)
    {
        await _context.WorkflowSteps.AddAsync(workflowStep);
    }

    public async Task<IReadOnlyList<WorkflowStep>> GetAllAsync()
    {
        return await _context.WorkflowSteps.ToListAsync();
    }

    public async Task<IReadOnlyList<Guid?>> GetByApproverIdAsync(Guid employeeId)
    {
        return await _context.WorkflowSteps
            .AsNoTracking()
            .Where(ws => ws.ApproverId == employeeId)
            .Select(ws => (Guid?)ws.InvoiceId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<WorkflowStep>> GetByEmployeeIdAsync(Guid employeeId)
    {
        return await _context.WorkflowSteps
            .AsNoTracking()
            .Where(ws => ws.ApproverId.Equals(employeeId))
            .ToListAsync();
    }

    public async Task<WorkflowStep?> GetByIdAsync(Guid id)
    {
        return await _context.WorkflowSteps.FindAsync(id);
    }

    public async Task<IReadOnlyList<WorkflowStep>> GetByInvoiceIdAsync(Guid invoiceId)
    {
        return await _context.WorkflowSteps
            .AsNoTracking()
            .Where(ws => ws.InvoiceId.Equals(invoiceId))
            .ToListAsync();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);

    }

}
