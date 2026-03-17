using Lupion.API;
using Lupion.API.Middlewares;
using Lupion.Business;
using Lupion.Business.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddBussinessDependencies();
builder.Services.AddDataDependencies(builder.Configuration);
builder.Services.AddAPIDependencies(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseResponseCompression();

app.UseAuthentication();

app.UseMiddleware<AuthorizationMiddleware>();

//app.UseMiddleware<RedisCacheMiddleware>();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
