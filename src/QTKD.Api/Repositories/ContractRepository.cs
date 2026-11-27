using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Models;

namespace QTKD.Api.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly QtkdDbContext db;

    public ContractRepository(QtkdDbContext db)
    {
        this.db = db;
    }

    public Task<List<Contract>> GetAllAsync()
    {
        return db.Contracts
            .AsNoTracking()
            .Include(contract => contract.Customer)
            .OrderBy(contract => contract.Id)
            .ToListAsync();
    }

    public Task<Contract?> GetByIdAsync(int id)
    {
        return db.Contracts
            .AsNoTracking()
            .Include(contract => contract.Customer)
            .FirstOrDefaultAsync(contract => contract.Id == id);
    }
}
