using InvoiceSystem.Domain.Entities;

namespace InvoiceSystem.Domain.Repositories;
public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Company>> GetAllAsync();

    Task<bool> ExistsByRegistrationNumberAsync(string registrationNumber);
    Task AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(Company company);
}
