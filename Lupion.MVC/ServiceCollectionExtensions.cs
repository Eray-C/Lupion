using Lupion.MVC.ActionFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Lupion.MVC;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMvcDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(configuration);
        services.AddHttpContextAccessor();
        services.AddControllersWithViews(options => { options.Filters.Add<JwtUserViewBagFilter>(); });
        services.Configure<AppSettings>(configuration);

        return services;
    }
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var accept = context.Request.Headers.Accept.ToString();
                    if (accept.Contains("text/html", StringComparison.OrdinalIgnoreCase) && !context.Response.HasStarted)
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/Login/Unauthorized");
                    }
                    return Task.CompletedTask;
                }
            };

            var validIssuer = configuration["JWT:Issuer"] ?? throw new Exception();
            var validAudience = configuration["JWT:Audience"] ?? throw new Exception();
            var issuerSigningKey = configuration["JWT:Key"] ?? throw new Exception();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = validIssuer,
                ValidAudience = validAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}
