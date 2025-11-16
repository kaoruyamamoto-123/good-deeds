using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

using Application;
using Infrastructure;
using Microsoft.AspNetCore.Localization;
using Persistence;

using WebAPI;
using WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddLogging();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistence(builder.Configuration);

// Setting CORS policy for local responds
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .SetIsOriginAllowed(_ => true)
              .SetIsOriginAllowedToAllowWildcardSubdomains()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    };
});

// Adding and configuration authentication and authorization by JWT Tokens
builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddBackgroundServices();

builder.Services.AddControllersWithViews();

// Adding and Configuration API Versioning
builder.Services.AddApiVersioning()
                .AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

builder.Services.AddHealthChecks();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var notAppliedMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (notAppliedMigrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
    catch
    {
        await dbContext.Database.MigrateAsync();
    }
}

app.UseExceptionHandler();

app.UseRouting();
app.UseCors("MyPolicy");

app.UseRequestLocalization();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always
});

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
