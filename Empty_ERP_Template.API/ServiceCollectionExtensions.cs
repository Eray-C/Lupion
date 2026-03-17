using Empty_ERP_Template.API;
using Empty_ERP_Template.API.ActionFilters;
using Empty_ERP_Template.API.JsonConverters;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Empty_ERP_Template.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAPIDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(configuration);
        services.AddHttpContextAccessor();

        services.AddControllers(options =>
        {
            options.Filters.Add<ModelValidationFilter>();
            options.Filters.Add<PutRequestValidationFilter>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new NullableDateTimeJsonConverter());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                      .WithOrigins("https://localhost:4000", "https://localhost:80", "https://cheetaherp.net")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("Content-Disposition", "X-File-Name");

            });
        });
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                new[] { "application/json" });
        });
        services.Configure<BrotliCompressionProviderOptions>(opt =>
        {
            opt.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        services.Configure<GzipCompressionProviderOptions>(opt =>
        {
            opt.Level = System.IO.Compression.CompressionLevel.Fastest;
        });


        services.AddScoped<ModelValidationFilter>();


        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var validIssuer = configuration["JWT:Issuer"] ?? throw new Exception("JWT Issuer not found");
        var validAudience = configuration["JWT:Audience"] ?? throw new Exception("JWT Audience not found");
        var issuerSigningKey = configuration["JWT:Key"] ?? throw new Exception("JWT Key not found");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
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
