using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
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

    public IQueryable<object> GetApproverTasksAsync(Guid employeeId, InvoiceStatus status)
    {
        return _context.WorkflowSteps
            .Where(ws => ws.ApproverId.Equals(employeeId) && ws.StatusAfter.Equals(status))
            .Join(_context.Invoices,
                    ws => ws.InvoiceId,
                    inv => inv.Id,
                    (ws, inv) => new { Step = ws, Invoice = inv });
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

    public IQueryable<WorkflowStep> QueryAll()
    {
        return _context.WorkflowSteps.AsQueryable();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);

    }

}
