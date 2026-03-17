using Lupion.Business;
using Lupion.Business.Middlewares;
using Lupion.MVC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBussinessDependencies();
builder.Services.AddDataDependencies(builder.Configuration);
builder.Services.AddMvcDependencies(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Login/StatusCode", "?code={0}");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuthorizationMiddleware>();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
