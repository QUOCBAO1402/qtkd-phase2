using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractService service;

    public ContractsController(IContractService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ContractResponse>>> GetAll()
    {
        List<ContractResponse> contracts = await service.GetAllAsync();
        return Ok(contracts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContractResponse>> GetById(int id)
    {
        ContractResponse? contract = await service.GetByIdAsync(id);
        if (contract == null)
        {
            return NotFound(new { message = $"Không tìm thấy hợp đồng có Id = {id}." });
        }

        return Ok(contract);
    }
}
