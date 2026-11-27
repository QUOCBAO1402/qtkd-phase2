using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService service;

    public CustomersController(ICustomerService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetAll()
    {
        List<CustomerResponse> customers = await service.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id)
    {
        CustomerResponse? customer = await service.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(new { message = $"Không tìm thấy khách hàng có Id = {id}." });
        }

        return Ok(customer);
    }
}
