using Microsoft.EntityFrameworkCore;
using QTKD.Api.Data;
using QTKD.Api.Models;

namespace QTKD.Api.Repositories;

public class PriceListRepository : IPriceListRepository
{
    private readonly QtkdDbContext db;

    public PriceListRepository(QtkdDbContext db)
    {
        this.db = db;
    }

    public Task<List<PriceList>> GetAllAsync()
    {
        return db.PriceLists
            .AsNoTracking()
            .Include(priceList => priceList.Customer)
            .Include(priceList => priceList.PriceItems.OrderBy(item => item.Id))
            .OrderBy(priceList => priceList.Id)
            .ToListAsync();
    }

    public Task<PriceList?> GetByIdAsync(int id)
    {
        return db.PriceLists
            .AsNoTracking()
            .Include(priceList => priceList.Customer)
            .Include(priceList => priceList.PriceItems.OrderBy(item => item.Id))
            .FirstOrDefaultAsync(priceList => priceList.Id == id);
    }
}
