using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Authentication;
using Empty_ERP_Template.Business.Exceptions;
using Empty_ERP_Template.Business.Requests.Authentication;
using Empty_ERP_Template.Data;
using Empty_ERP_Template.Data.Entities.AuthenticationEntities;
using Microsoft.EntityFrameworkCore;

namespace Empty_ERP_Template.Business.Services;

public class SettingsService(DBContext context, ManagementDBContext managementContext, IMapper mapper)
{
   

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var roles = await context.Roles.Where(x => !x.IsDeleted).ToListAsync();
        return mapper.Map<IEnumerable<RoleDTO>>(roles);
    }

    public async Task<int> CreateRoleAsync(RoleRequest request)
    {
        var entity = new Role { Name = request.Name ?? "", Description = request.Description ?? "" };
        context.Roles.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateRoleAsync(int id, RoleRequest request)
    {
        var entity = await context.Roles.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.Name = request.Name;
        entity.Description = request.Description ?? "";
        await context.SaveChangesAsync();
    }

    public async Task DeleteRoleAsync(int id)
    {
        var entity = await context.Roles.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
    }


    public async Task<IEnumerable<RolePermissionItemDTO>> GetRolePermissionsAsync(int roleId)
    {
        var modules = await managementContext.Modules
            .Include(m => m.ModuleOperations)
            .OrderBy(m => m.Name)
            .ToListAsync();


        var operationIds = (await context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.ModuleOperationId)
            .ToListAsync())
            .ToHashSet();

        var join = modules
            .SelectMany(mod => (mod.ModuleOperations ?? [])
            .Select(op => new { Module = mod, Operation = op }))
            .OrderBy(x => x.Module.Name)
            .ThenBy(x => x.Operation.Name)
            .ToList();


        return join.Select(x =>
        {
            return new RolePermissionItemDTO
            {
                ModuleId = x.Module.Id,
                ModuleName = x.Module.Name ?? "",
                OperationId = x.Operation.Id,
                OperationName = x.Operation.Name ?? "",
                Description = x.Operation.Description,
                Checked = operationIds.Contains(x.Operation.Id)
            };
        }).ToList();
    }

    public async Task SaveRolePermissionsAsync(SaveRolePermissionsRequest request)
    {
        var existing = await context.RolePermissions.Where(rp => rp.RoleId == request.RoleId).ToListAsync();
        context.RolePermissions.RemoveRange(existing);

        var toAdd = (request.GridItems ?? [])
            .Where(x => x.Checked && x.OperationId > 0)
            .Select(x => new RolePermission { RoleId = request.RoleId, ModuleOperationId = x.OperationId })
            .ToList();
        foreach (var item in toAdd)
            context.RolePermissions.Add(item);
        await context.SaveChangesAsync();
    }
}
