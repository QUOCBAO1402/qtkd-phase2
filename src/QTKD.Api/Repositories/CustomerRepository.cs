using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Models;

namespace QTKD.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly QtkdDbContext db;

    public CustomerRepository(QtkdDbContext db)
    {
        this.db = db;
    }

    public Task<List<Customer>> GetAllAsync()
    {
        return db.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Id)
            .ToListAsync();
    }

    public Task<Customer?> GetByIdAsync(int id)
    {
        return db.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == id);
    }
}
