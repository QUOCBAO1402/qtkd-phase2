using QTKD.Api.DTOs;
using QTKD.Api.Models;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class ContractService : IContractService
{
    private readonly IContractRepository repository;

    public ContractService(IContractRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<ContractResponse>> GetAllAsync()
    {
        List<Contract> contracts = await repository.GetAllAsync();
        return contracts.Select(ToResponse).ToList();
    }

    public async Task<ContractResponse?> GetByIdAsync(int id)
    {
        Contract? contract = await repository.GetByIdAsync(id);
        return contract == null ? null : ToResponse(contract);
    }

    private static ContractResponse ToResponse(Contract contract)
    {
        return new ContractResponse
        {
            Id = contract.Id,
            ContractNo = contract.ContractNo,
            CustomerId = contract.CustomerId,
            CustomerName = contract.Customer?.Name ?? string.Empty,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            Value = contract.Value,
            PaymentTerms = contract.PaymentTerms,
            ServiceTerms = contract.ServiceTerms,
            AttachmentUrl = contract.AttachmentUrl,
            Status = contract.Status,
            SubmittedAt = contract.SubmittedAt,
            CreatedAt = contract.CreatedAt
        };
    }
}
