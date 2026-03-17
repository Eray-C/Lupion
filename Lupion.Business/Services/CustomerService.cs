using AutoMapper;
using Lupion.Business.DTOs.Customer;
using Lupion.Business.Exceptions;
using Lupion.Business.Requests.Customer;
using Lupion.Data;
using Lupion.Data.Entities.CustomerEntities;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Business.Services;

public class CustomerService(DBContext context, DBContextFactory contextFactory, IMapper mapper)
{
    public async Task<IEnumerable<CustomerDTO>> GetCustomersAsync()
    {
        var entities = await context.Customers.AsNoTracking().ToListAsync();
        return mapper.Map<IEnumerable<CustomerDTO>>(entities);
    }
    public async Task<CustomerDTO> GetCustomerByIdAsync(int customerId)
    {
        await using var db = contextFactory.CreateDBContext();
        var entity = await db.Customers
                             .Where(x => x.Id == customerId)
                             .AsNoTracking()
                             .FirstOrDefaultAsync();

        return mapper.Map<CustomerDTO>(entity);
    }


    public async Task<int> AddAsync(CustomerRequest request)
    {
        var entity = mapper.Map<Customer>(request);
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(int id, CustomerRequest request)
    {
        var existing = await context.Customers.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, existing);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Customers.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CustomerContractDTO>> GetContractsAsync(int customerId)
    {
        await using var db = contextFactory.CreateDBContext();
        var list = await db.CustomerContracts.Where(x => x.CustomerId == customerId).AsNoTracking().ToListAsync();
        return mapper.Map<IEnumerable<CustomerContractDTO>>(list);
    }

    public async Task<int> AddContractAsync(CustomerContractRequest request)
    {
        var entity = mapper.Map<CustomerContract>(request);
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateContractAsync(int id, CustomerContractRequest request)
    {
        var entity = await context.CustomerContracts.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteContractAsync(int id)
    {
        var entity = await context.CustomerContracts.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }

    public async Task<CustomerAllDataDTO> GetCustomerAllDataAsync(int customerId)
    {
        var customerTask = GetCustomerByIdAsync(customerId);
        var contractsTask = GetContractsAsync(customerId);
        var pricesTask = GetPricesAsync(customerId);

        await Task.WhenAll(customerTask, contractsTask);

        return new CustomerAllDataDTO
        {
            Customer = await customerTask,
            CustomerContracts = await contractsTask,
            Prices = await pricesTask,
        };
    }




    public async Task<int> AddPriceAsync(CustomerPriceRequest request)
    {
        var entity = mapper.Map<CustomerPrice>(request);
        await context.AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdatePriceAsync(int id, CustomerPriceRequest request)
    {
        var entity = await context.CustomerPrices.FindAsync(id) ?? throw new RecordNotFoundException();
        mapper.Map(request, entity);
        await context.SaveChangesAsync();
    }

    public async Task DeletePriceAsync(int id)
    {
        var entity = await context.CustomerPrices.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }
    public async Task<IEnumerable<CustomerPriceDTO>> GetPricesAsync(int customerId)
    {
        await using var db = contextFactory.CreateDBContext();
        var list = await db.CustomerPrices
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

        return mapper.Map<IEnumerable<CustomerPriceDTO>>(list);
    }
}
