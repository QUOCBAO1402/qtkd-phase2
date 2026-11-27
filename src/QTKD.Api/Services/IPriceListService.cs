using QTKD.Api.DTOs;

namespace QTKD.Api.Services;

public interface IPriceListService
{
    Task<List<PriceListResponse>> GetAllAsync();
    Task<PriceListResponse?> GetByIdAsync(int id);
}
