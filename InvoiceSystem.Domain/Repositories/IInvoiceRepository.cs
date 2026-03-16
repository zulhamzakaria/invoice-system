using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.SharedContracts;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Invoice>> GetAllAsync();
    Task<bool> ExistsByInvoiceNoAsync(string invoiceNo);
    Task<IReadOnlyList<Invoice>> GetByCreatedByIdAsync(Guid employeeId);
    IQueryable<Invoice> QueryAll();
    Task AddAsync(Invoice invoice);
    Task AddItemAsync(InvoiceItem item);
    Task UpdateAsync(Invoice invoice);
    Task DeleteAsync(Invoice invoice);
    Task<int> SaveChangesAsync(CancellationToken token = default);
}
