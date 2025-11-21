using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;

public interface IWorkflowStepRepository
{
    Task<WorkflowStep?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<WorkflowStep>> GetAllAsync();
    Task<IReadOnlyList<Guid?>> GetByApproverId(Guid employeeId);
    Task AddAsync(WorkflowStep workflowStep);
    //Task UpdateAsync(WorkflowStep workflowStep);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

