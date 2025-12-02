using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

namespace InvoiceSystem.Domain.Repositories;

public interface IWorkflowStepRepository
{
    Task<WorkflowStep?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<WorkflowStep>> GetAllAsync();
    Task<IReadOnlyList<Guid?>> GetByApproverIdAsync(Guid employeeId);
    Task<IReadOnlyList<WorkflowStep>> GetByEmployeeIdAsync(Guid employeeId);
    IQueryable<object> GetApproverTasksAsync(Guid employeeId, InvoiceStatus status);
    Task<IReadOnlyList<WorkflowStep>> GetByInvoiceIdAsync(Guid invoiceId);
    Task AddAsync(WorkflowStep workflowStep);
    //Task UpdateAsync(WorkflowStep workflowStep);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

