using InvoiceSystem.Domain.Entities;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Invoice>> GetAllAsync();
    Task AddAsync(Invoice invoice);
    Task AddItemAsync(InvoiceItem item);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
