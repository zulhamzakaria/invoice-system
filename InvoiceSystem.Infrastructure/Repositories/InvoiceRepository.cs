using InvoiceSystem.Domain.Common;
using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            //await _context.SaveChangesAsync();
        }

        public async Task AddItemAsync(InvoiceItem item)
        {
            await _context.InvoiceItems.AddAsync(item);
        }

        public async Task DeleteAsync(Invoice invoice)
        {
            // ⚠️ In some domains, "Delete" is really "Void" instead of removing from DB
            _context.Invoices.Remove(invoice);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByInvoiceNoAsync(string invoiceNo)
        {
            return await _context.Invoices.
                AnyAsync(i => i.InvoiceNumber == invoiceNo && i.Status != InvoiceStatus.Voided);
        }

        public async Task<IReadOnlyList<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .Include(i => i.InvoiceItems)        // include invoice items
                .Include(i => i.Company)      // include company
                .ToListAsync();
        }

        public async Task<Invoice?> GetByIdAsync(Guid id)
        {
            return await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> SaveChangesAsync(CancellationToken token = default)
        {

            foreach (var entry in _context.ChangeTracker.Entries<InvoiceItem>())
            {
                if (entry.Entity.Quantity <= 0)
                    throw new Exception("somn wrong");
            }

            return await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            //await _context.SaveChangesAsync();
        }
    }
}
