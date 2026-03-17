using Lupion.Business.Exceptions;
using Lupion.Business.Requests.Authentication;
using Lupion.Data;
using Lupion.Data.Entities.AuthenticationEntities;
using Lupion.Data.Entities.SharedEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lupion.Business.Services;

public class AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, DBContext context, ManagementDBContext managementDBContext, EmailService emailService) : BaseService(httpContextAccessor)
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> LoginLocks = new();

    public async Task<string> LoginAsync(LoginRequest request)
    {
        var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        var key = $"{ip}:{request.EmailOrUserName}".ToLowerInvariant();
        var sem = LoginLocks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        if (!await sem.WaitAsync(TimeSpan.Zero))
            throw new LoginInProgressException();

        try
        {
            var user = await managementDBContext.Users
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => (x.Email == request.EmailOrUserName || x.Username == request.EmailOrUserName) && x.IsActive);

            if (user is null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("GeÃ§ersiz kullanÄ±cÄ± adÄ± veya ÅŸifre");
            }

            return GenerateJwtToken(user);
        }
        finally
        {
            sem.Release();
        }
    }
    private static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        return computedHash.SequenceEqual(storedHash);
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var (hash, salt) = CreatePasswordHash(request.Password);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsActive = request.IsActive,
            CompanyCode = CurrentUser.CompanyCode,
        };

        using var transaction = await managementDBContext.Database.BeginTransactionAsync();


        await managementDBContext.Users.AddAsync(user);
        await managementDBContext.SaveChangesAsync();

        if (request.RoleIds != null && request.RoleIds.Count != 0)
        {
            await AddUserRolesAsync(user.Id, request.RoleIds);

            await context.SaveChangesAsync();
        }

        await transaction.CommitAsync();

    }
    public async Task UpdateAsync(int id, RegisterRequest request)
    {
        var user = await managementDBContext.Users.FindAsync(id) ?? throw new RecordNotFoundException();

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Username = request.Username;
        user.Email = request.Email;
        user.IsActive = request.IsActive;

        var existingRoles = await context.UserRoles
            .Where(x => x.UserId == user.Id)
            .ToListAsync();

        context.UserRoles.RemoveRange(existingRoles);

        foreach (var roleId in request.RoleIds)
        {
            context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = roleId
            });
        }

        await managementDBContext.SaveChangesAsync();
        await context.SaveChangesAsync();
    }


    private static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
    {
        using var hmac = new HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

    private async Task AddUserRolesAsync(int userId, IEnumerable<int> roleIds)
    {
        var userRoles = roleIds.Select(roleId => new UserRole
        {
            UserId = userId,
            RoleId = roleId
        });

        await context.UserRoles.AddRangeAsync(userRoles);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = CreateClaims(user);

        var JWTkey = configuration["JWT:Key"];
        var issuer = configuration["JWT:Issuer"];
        var audience = configuration["JWT:Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTkey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static List<Claim> CreateClaims(User user)
    {
        var roles = GetUserRoleNames(user);

        List<Claim> claims = [
            ..roles.Select(x => new Claim(ClaimTypes.Role, x)),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName),
            new("CompanyCode", user.CompanyCode)
        ];

        return claims;
    }

    private static List<string> GetUserRoleNames(User user)
    {
        var options = new DbContextOptionsBuilder<DBContext>()
            .UseSqlServer(user.Company.ConnectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .Options;

        using var context = new DBContext(options);

        var rolesNames = context.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.Role.Name).ToList();

        return rolesNames;
    }



    private const int ResetCodeExpiryMinutes = 10;
    private const int ResetCodeMaxRequestsPerWindow = 3;
    private static readonly TimeSpan ResetRequestWindow = TimeSpan.FromMinutes(15);
    private const int ResetCodeMaxFailedAttempts = 5;

    public async Task GeneratePasswordResetTokenAsync(ForgotPasswordRequest request)
    {
        var user = await managementDBContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.IsActive);
        if (user is null)
            throw new Exception("KullanÄ±cÄ± bulunamadÄ±");

        var since = DateTime.UtcNow.Subtract(ResetRequestWindow);
        var recentCount = await managementDBContext.PasswordResetTokens
            .CountAsync(x => x.UserId == user.Id && x.CreatedAt >= since);
        if (recentCount >= ResetCodeMaxRequestsPerWindow)
            throw new Exception("Ã‡ok fazla deneme. LÃ¼tfen 15 dakika sonra tekrar deneyin.");

        var code = Random.Shared.Next(100000, 999999).ToString();
        var expiresAt = DateTime.UtcNow.AddMinutes(ResetCodeExpiryMinutes);
        var resetToken = new PasswordResetToken(user.Id, code, expiresAt);

        await managementDBContext.PasswordResetTokens.AddAsync(resetToken);
        await managementDBContext.SaveChangesAsync();

        await emailService.SendPasswordResetAsync(request.Email, code);
    }

    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token) || request.Token.Length != 6 || !request.Token.All(char.IsDigit))
            throw new Exception("GeÃ§ersiz doÄŸrulama kodu formatÄ±.");

        var user = await managementDBContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email && x.IsActive);
        if (user is null)
            throw new Exception("KullanÄ±cÄ± bulunamadÄ±.");

        var resetToken = await managementDBContext.PasswordResetTokens
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Token == request.Token && !x.Used);
        if (resetToken is null)
        {
            var activeToken = await managementDBContext.PasswordResetTokens
                .Where(x => x.UserId == user.Id && !x.Used && x.ExpiresAt >= DateTime.UtcNow)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();
            if (activeToken != null)
            {
                activeToken.FailedAttempts++;
                await managementDBContext.SaveChangesAsync();
                if (activeToken.FailedAttempts >= ResetCodeMaxFailedAttempts)
                    throw new Exception("Kod bloke edildi. LÃ¼tfen yeni kod talep edin.");
            }
            throw new Exception("GeÃ§ersiz doÄŸrulama kodu.");
        }

        if (resetToken.ExpiresAt < DateTime.UtcNow)
            throw new Exception("DoÄŸrulama kodunun sÃ¼resi dolmuÅŸ. LÃ¼tfen yeni kod talep edin.");

        if (resetToken.FailedAttempts >= ResetCodeMaxFailedAttempts)
            throw new Exception("Kod bloke edildi. LÃ¼tfen yeni kod talep edin.");

        var (hash, salt) = CreatePasswordHash(request.Password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        resetToken.Used = true;
        await managementDBContext.SaveChangesAsync();
    }
}
