using InvoiceSystem.Domain.Entities;
using InvoiceSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            //await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            //await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeExists(string email)
        {
            return await _context.Employees.AsNoTracking().AnyAsync(e=> e.Email == email && e.Status == EmployeeStatus.Active);
        }

        public async Task<IReadOnlyList<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<IReadOnlyList<Employee>> GetByTypeAsync(EmployeeType type)
        {
            IQueryable<Employee> query = _context.Employees;

            query = type switch
            {
                EmployeeType.Clerk => query.OfType<Clerk>(),
                EmployeeType.FO => query.OfType<FO>(),
                EmployeeType.FM => query.OfType<FM>(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), "Undefined EmployeeType")
            };

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            //await _context.SaveChangesAsync();
        }
    }
}
