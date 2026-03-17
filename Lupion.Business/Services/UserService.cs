using AutoMapper;
using Lupion.Business.DTOs.Authentication;
using Lupion.Business.Exceptions;
using Lupion.Data;
using Lupion.Data.Entities.AuthenticationEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lupion.Business.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, ManagementDBContext managementDBContext,
    DBContext context, IMapper mapper, CacheService cacheService)
{
    private const string EndpointPermissionCacheKeyPrefix = "Auth:EndpointPermission:";
    private const string UserPermissionsCacheKeyPrefix = "Auth:UserPermissions:";

    public async Task<int?> GetRequiredModuleOperationIdAsync(string path, string method)
    {
        var cacheKey = $"{EndpointPermissionCacheKeyPrefix}Path={path}|Method={method}";
        return await cacheService.GetOrAddAsync(cacheKey, async _ =>
        {
            var permission = await managementDBContext.ApiPermissions
                .FirstOrDefaultAsync(p => p.Path == path && p.Method == method);
           
            return permission?.ModuleOperationId;
        });
    }

   



    public async Task<HashSet<int>> GetUserRolePermissionsAsync(int userId)
    {
        var cacheKey = $"{UserPermissionsCacheKeyPrefix}UserId={userId}";
        var userRolePermissions = await cacheService.GetOrAddAsync(cacheKey, async _ =>
        {
            var permissions = await context.UserRoles
                    .Where(ur => ur.UserId == userId && !ur.IsDeleted)
                    .Join(context.RolePermissions, ur => ur.RoleId, rp => rp.RoleId, (ur, rp) => rp.ModuleOperationId)
                    .Distinct()
                    .ToHashSetAsync();
            return permissions;
        });

        return userRolePermissions;
    }
    public async Task<IEnumerable<UserDTO>> QueryUsersAsync()
    {
        var currentUser = httpContextAccessor.HttpContext.User;
        var companyCode = currentUser.FindFirst("CompanyCode")?.Value;

        var users = await managementDBContext.Users
            .Where(x => x.CompanyCode == companyCode)
            .ToListAsync();

        var userDtos = new List<UserDTO>();

        foreach (var user in users)
        {
            var userRoleIds = await context.UserRoles
                .Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId)
                .ToListAsync();

            var dto = mapper.Map<UserDTO>(user);
            dto.RoleIds = userRoleIds;

            userDtos.Add(dto);
        }

        return userDtos;
    }


    public async Task<bool> IsAnyUserWithThisEmail(string email, int excludedId)
    {
        return await managementDBContext.Users.AnyAsync(x => x.Email == email && x.Id != excludedId);
    }

    public async Task<bool> IsAnyUserWithThisUsername(string username, int excludedId)
    {
        return await managementDBContext.Users.AnyAsync(x => x.Username == username && x.Id != excludedId);
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await managementDBContext.Users.FindAsync(id) ?? throw new RecordNotFoundException();
        entity.IsDeleted = true;
        await managementDBContext.SaveChangesAsync();
    }

}
