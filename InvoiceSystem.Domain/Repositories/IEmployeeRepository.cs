using InvoiceSystem.Domain.Entities;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Employee>> GetAllAsync();
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
