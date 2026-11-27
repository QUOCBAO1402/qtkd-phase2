using QTKD.Api.Models;

namespace QTKD.Api.Repositories;

public interface IPriceListRepository
{
    Task<List<PriceList>> GetAllAsync();
    Task<PriceList?> GetByIdAsync(int id);
}
