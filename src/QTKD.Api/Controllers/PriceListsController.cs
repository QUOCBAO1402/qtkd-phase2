using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/pricelists")]
public class PriceListsController : ControllerBase
{
    private readonly IPriceListService service;

    public PriceListsController(IPriceListService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PriceListResponse>>> GetAll()
    {
        List<PriceListResponse> priceLists = await service.GetAllAsync();
        return Ok(priceLists);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PriceListResponse>> GetById(int id)
    {
        PriceListResponse? priceList = await service.GetByIdAsync(id);
        if (priceList == null)
        {
            return NotFound(new { message = $"Không tìm thấy bảng giá có Id = {id}." });
        }

        return Ok(priceList);
    }
}
