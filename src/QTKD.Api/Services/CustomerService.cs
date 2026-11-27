using QTKD.Api.DTOs;
using QTKD.Api.Models;
using QTKD.Api.Repositories;

namespace QTKD.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository repository;

    public CustomerService(ICustomerRepository repository)
    {
        this.repository = repository;
    }

    public async Task<List<CustomerResponse>> GetAllAsync()
    {
        List<Customer> customers = await repository.GetAllAsync();
        return customers.Select(ToResponse).ToList();
    }

    public async Task<CustomerResponse?> GetByIdAsync(int id)
    {
        Customer? customer = await repository.GetByIdAsync(id);
        return customer == null ? null : ToResponse(customer);
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            CustomerCode = customer.CustomerCode,
            Name = customer.Name,
            TaxCode = customer.TaxCode,
            Address = customer.Address,
            Representative = customer.Representative,
            ContactInfo = customer.ContactInfo,
            Status = customer.Status,
            CreatedAt = customer.CreatedAt
        };
    }
}
