using Lupion.Data.Entities;
using Lupion.Data.Entities.AttachmentEntities;
using Lupion.Data.Entities.AuthenticationEntities;
using Lupion.Data.Entities.CustomerEntities;
using Lupion.Data.Entities.PersonnelEntities;
using Lupion.Data.Entities.SharedEntities;
using Lupion.Data.Entities.TaskManagerEntities;
using Lupion.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Data;

public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
{
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Menu> Menus { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<GeneralType> GeneralTypes { get; set; }
    public DbSet<Attachments> Attachments { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Personnel> Personnels { get; set; }
    public DbSet<PersonnelLicense> PersonnelLicenses { get; set; }
    public DbSet<PersonnelSalary> PersonnelSalaries { get; set; }
    public DbSet<PersonnelBonus> PersonnelBonuses { get; set; }
    public DbSet<PersonnelDeduction> PersonnelDeductions { get; set; }
    public DbSet<PersonnelAdvance> PersonnelAdvances { get; set; }
    public DbSet<PersonnelAdvancePayment> PersonnelAdvancePayments { get; set; }
    public DbSet<PersonnelPaymentHistory> PersonnelPaymentHistory { get; set; }
    public DbSet<PaidPayroll> PaidPayrolls { get; set; }
    public DbSet<PersonnelPayroll> PersonnelPayrolls { get; set; }
    public DbSet<PersonnelContact> PersonnelContacts { get; set; }
    public DbSet<PersonnelRelativeContact> PersonnelRelativeContacts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerContract> CustomerContracts { get; set; }
    public DbSet<CustomerPrice> CustomerPrices { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<TaskHistory> TaskHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DBContext).Assembly);

        modelBuilder.ApplySoftDeleteFilter();
    }
}
