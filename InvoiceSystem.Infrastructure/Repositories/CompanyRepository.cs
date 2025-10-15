using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
        => await _context.Companies
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<IReadOnlyList<Company>> GetAllAsync()
        => await _context.Companies
            .Include(c => c.Addresses)
            .ToListAsync();

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Company company)
    {
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByRegistrationNumberAsync(string registrationNumber)
    {
        return await _context.Companies.AnyAsync(company => company.RegistrationNumber == registrationNumber);
    }
}
