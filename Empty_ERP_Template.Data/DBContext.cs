using Empty_ERP_Template.Data.Entities;
using Empty_ERP_Template.Data.Entities.AttachmentEntities;
using Empty_ERP_Template.Data.Entities.AuthenticationEntities;
using Empty_ERP_Template.Data.Entities.CustomerEntities;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;
using Empty_ERP_Template.Data.Entities.SharedEntities;
using Empty_ERP_Template.Data.Entities.TaskManagerEntities;
using Empty_ERP_Template.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Empty_ERP_Template.Data;

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
