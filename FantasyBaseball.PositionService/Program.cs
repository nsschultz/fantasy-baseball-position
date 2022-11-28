using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var BaseballSpecificOrigins = "_BaseballSpecificOrigins";
var SwaggerBasePath = "api/v1/position";
var SwaggerTitle = "FantasyBaseball.PositionService";
var SwaggerVersion = "v1";

// Build the App Config
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
// Setup Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: BaseballSpecificOrigins,
        policy =>
        {
            policy.SetIsOriginAllowed(o =>
            {
                var host = new Uri(o).Host;
                return host == "localhost" || host.Contains("schultz.local");
            });
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});
// Setup Database
var connectionString = string.Format(
    builder.Configuration.GetConnectionString("PositionDatabase"),
    builder.Configuration["POSITION_DATABASE_HOST"],
    builder.Configuration["POSITION_DATABASE"],
    builder.Configuration["POSITION_DATABASE_USER"],
    builder.Configuration["POSITION_DATABASE_PASSWORD"]);
builder.Services.AddDbContext<PositionContext>(options => options.UseNpgsql(connectionString));
// Setup HealthChecks
builder.Services.AddHealthChecks().AddDbContextCheck<PositionContext>();
// Setup Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Setup DI
builder.Services
    // Config
    .AddSingleton(builder.Configuration)
    // Context
    .AddScoped<IPositionContext>(provider => provider.GetService<PositionContext>())
    // Repos
    .AddScoped<IPositionRepository, PositionRepository>()
    // Services
    .AddScoped<IGetPositionsService, GetPositionsService>();
// Setup Swagger
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc(SwaggerVersion, new OpenApiInfo { Title = SwaggerTitle, Version = SwaggerVersion });
    var currentAssembly = Assembly.GetExecutingAssembly();
    currentAssembly.GetReferencedAssemblies()
        .Union(new AssemblyName[] { currentAssembly.GetName() })
        .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))
        .Where(f => File.Exists(f))
        .ToList()
        .ForEach(f => o.IncludeXmlComments(f));
});
// Setup Controllers
builder.Services.AddControllers();

// Build the App
var app = builder.Build();
app.UseCors(BaseballSpecificOrigins);
app.UseHsts();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.MapHealthChecks("/api/health", new HealthCheckOptions { AllowCachingResponses = false });
app.UseSwagger(c => c.RouteTemplate = SwaggerBasePath + "/swagger/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"/{SwaggerBasePath}/swagger/{SwaggerVersion}/swagger.json", $"{SwaggerTitle} - {SwaggerVersion}");
    c.RoutePrefix = $"{SwaggerBasePath}/swagger";
});
// Migrate Database on Startup
using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<PositionContext>().Database.Migrate();
// Start the App
app.Run();