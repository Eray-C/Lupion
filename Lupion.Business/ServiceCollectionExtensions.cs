using Lupion.Business.Helpers;
using Lupion.Business.Infrastructure;
using Lupion.Business.MapProfiles;
using Lupion.Business.Repository;
using Lupion.Business.Services;
using Lupion.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lupion.Business;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBussinessDependencies(this IServiceCollection services)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "194.62.55.36:6379,password=ErfaCheetahRedis?";
        });

        services.AddMemoryCache();
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<CompanyService>();
        services.AddScoped<IsAnyExist>();
        services.AddScoped<CacheService>();
        services.AddScoped<MenuService>();
        services.AddScoped<SharedService>();
        services.AddScoped<SettingsService>();
        services.AddScoped<AuthService>();
        services.AddScoped<EmailService>();
        services.AddScoped<BaseService>();
        services.AddScoped<UserService>();
        services.AddScoped<PersonnelService>();
        services.AddScoped<CustomerService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<TaskManagerService>();
        services.AddScoped(typeof(Repository<>));
        services.AddScoped<DBContextFactory>();
        services.AddSingleton<MinioService>();
        services.AddScoped<AttachmentService>();

        services.AddScoped<TenantContext>();
        //services.AddScoped<RedisCacheService>();
        //services.AddScoped<EntityCacheInvalidationInterceptor>();


        return services;
    }
    public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddManagementDBContext(configuration);
        services.AddDBContext();

        return services;
    }
    private static IServiceCollection AddManagementDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ManagementDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("ManagementDB")).LogTo(Console.WriteLine, LogLevel.Information));

        return services;
    }
    private static IServiceCollection AddDBContext(this IServiceCollection services)
    {
        services.AddScoped<DBContext>(serviceProvider =>
        {
            var connectionString = serviceProvider.GetConnectionString();
            //var interceptor = serviceProvider.GetRequiredService<EntityCacheInvalidationInterceptor>();

            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();

            if (connectionString != default)
            {
                optionsBuilder
                    .UseSqlServer(connectionString)
                    //.AddInterceptors(interceptor)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            }

            return new DBContext(optionsBuilder.Options);
        });

        return services;
    }

    public static string? GetConnectionString(this IServiceProvider serviceProvider)
    {
        var user = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            var companyCode = user.FindFirst("CompanyCode").Value;

            var companyService = serviceProvider.GetRequiredService<CompanyService>();
            var company = companyService.GetCompanyByCode(companyCode) ?? throw new Exception("Tenant bulunamadÄ±");

            return company.ConnectionString;
        }

        return default;
    }
}
