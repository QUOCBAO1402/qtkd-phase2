using QTKD.Api.DTOs;

namespace QTKD.Api.Services;

public interface IContractService
{
    Task<List<ContractResponse>> GetAllAsync();
    Task<ContractResponse?> GetByIdAsync(int id);
}
