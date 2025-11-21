using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);
    Task<bool> EmployeeExists(string email);
    Task<IReadOnlyList<Employee>> GetAllAsync();
    Task<IReadOnlyList<Employee>> GetByTypeAsync(EmployeeType type);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Employee employee);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
