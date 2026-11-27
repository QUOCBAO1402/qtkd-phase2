using QTKD.Api.Models;

namespace QTKD.Api.Repositories;

public interface IContractRepository
{
    Task<List<Contract>> GetAllAsync();
    Task<Contract?> GetByIdAsync(int id);
}
