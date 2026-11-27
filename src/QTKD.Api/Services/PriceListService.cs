using QTKD.Api.DTOs;
using QTKD.Api.Models;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class PriceListService : IPriceListService
{
    private readonly IPriceListRepository repository;

    public PriceListService(IPriceListRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<PriceListResponse>> GetAllAsync()
    {
        List<PriceList> priceLists = await repository.GetAllAsync();
        return priceLists.Select(ToResponse).ToList();
    }

    public async Task<PriceListResponse?> GetByIdAsync(int id)
    {
        PriceList? priceList = await repository.GetByIdAsync(id);
        return priceList == null ? null : ToResponse(priceList);
    }

    private static PriceListResponse ToResponse(PriceList priceList)
    {
        return new PriceListResponse
        {
            Id = priceList.Id,
            Name = priceList.Name,
            CustomerId = priceList.CustomerId,
            CustomerName = priceList.Customer?.Name,
            EffectiveDate = priceList.EffectiveDate,
            EndDate = priceList.EndDate,
            Status = priceList.Status,
            CreatedAt = priceList.CreatedAt,
            PriceItems = priceList.PriceItems.Select(item => new PriceItemResponse
            {
                Id = item.Id,
                PriceListId = item.PriceListId,
                ServiceCode = item.ServiceCode,
                ServiceName = item.ServiceName,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice
            }).ToList()
        };
    }
}
