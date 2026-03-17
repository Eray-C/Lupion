using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Shared;
using Empty_ERP_Template.Business.Exceptions;
using Empty_ERP_Template.Business.Requests.Shared;
using Empty_ERP_Template.Data;
using Empty_ERP_Template.Data.Entities.SharedEntities;
using Microsoft.EntityFrameworkCore;

namespace Empty_ERP_Template.Business.Services;

public class SharedService(DBContext context, CacheService cacheService, IMapper mapper)
{
    public async Task<IEnumerable<GeneralTypeDTO>> QueryTypesAsync(string category, int? parentId = null)
    {
        var types = await GetTypesAsync();

        var filteredTypes = types
            .Where(x => x.Category == category)
            .Where(x => parentId == null || x.ParentId == parentId)
            .OrderBy(x => x.Order == null)
            .ThenBy(x => x.Order)
            .ToList();

        return mapper.Map<IEnumerable<GeneralTypeDTO>>(filteredTypes);
    }

    public async Task<IEnumerable<Currency>> GetCurrencies()
    {
        var currencies = await context.Currencies.ToListAsync();

        return currencies;
    }
    public async Task<int> AddTypeAsync(GeneralTypeRequest request)
    {
        var type = mapper.Map<GeneralType>(request);

        await context.GeneralTypes.AddAsync(type);
        await context.SaveChangesAsync();

        return type.Id;
    }
    public async Task UpdateTypeAsync(int id, GeneralTypeRequest request)
    {
        var existingEntity = await context.GeneralTypes.FindAsync(id) ?? throw new RecordNotFoundException();

        mapper.Map(request, existingEntity);

        await context.SaveChangesAsync();
    }
    public async Task DeleteTypeAsync(int id)
    {
        var entity = await context.GeneralTypes.FindAsync(id) ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;

        await context.SaveChangesAsync();
    }
    public async Task<IEnumerable<GeneralType>> GetTypesFromCacheAsync()
    {
        return await cacheService.GetOrAddAsync("Types", async x =>
        {
            return await GetTypesAsync();
        });
    }
    public async Task<IEnumerable<GeneralType>> GetTypesAsync()
    {
        return await context.GeneralTypes.ToListAsync();
    }
}
