namespace InvoiceSystem.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
