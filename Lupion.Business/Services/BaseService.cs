using Lupion.Business.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Lupion.Business.Services;

public class BaseService(IHttpContextAccessor httpContextAccessor)
{
    private HttpContext HttpContext => httpContextAccessor.HttpContext;
    private ClaimsPrincipal ClaimUser => HttpContext.User;

    private UserDTO? _currentUser;
    public UserDTO CurrentUser => _currentUser ??= new()
    {
        Id = int.Parse(ClaimUser.FindFirst(ClaimTypes.NameIdentifier)!.Value),
        FirstName = ClaimUser.FindFirst("FirstName")!.Value,
        LastName = ClaimUser.FindFirst("LastName")!.Value,
        Email = ClaimUser.FindFirst(ClaimTypes.Email)!.Value,
        Username = ClaimUser.FindFirst(ClaimTypes.Name)!.Value,
        CompanyCode = ClaimUser.FindFirst("CompanyCode")!.Value,
        Roles = ClaimUser.FindAll(ClaimTypes.Role).Select(x => x.Value),
    };
}