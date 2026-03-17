using Lupion.Data.Entities;
using Lupion.Data.Entities.AuthenticationEntities;
using Lupion.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Data;

public class ManagementDBContext(DbContextOptions<ManagementDBContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<ModuleOperation> ModuleOperations { get; set; }
    public DbSet<ApiPermission> ApiPermissions { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<EmailAccount> EmailAccounts { get; set; }
    public DbSet<HttpLog> HttpLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagementDBContext).Assembly);

        modelBuilder.ApplySoftDeleteFilter();
    }
}
