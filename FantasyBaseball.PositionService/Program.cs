using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FantasyBaseball.Common.Exceptions;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Database.Repositories;
using FantasyBaseball.PositionService.Maps;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

// Position Service Application Entry Point Configuration
// This is a read-only ASP.NET Core Web API that serves as the source of truth for position information.
// The service provides fantasy baseball position data including batter and pitcher positions,
// position hierarchies, and additional eligible positions for each position type.
//
// Configuration Details:
// - CORS is configured to allow requests from localhost and schultz.local hosts
// - Database connectivity is required for health checks
// - AutoMapper is configured for Position entity to DTO mapping
// - Swagger/OpenAPI documentation is available at /api/swagger/

const string BaseballSpecificOrigins = "_BaseballSpecificOrigins";
const string SwaggerBasePath = "api";
const string SwaggerTitle = "FantasyBaseball.PositionService";
const string SwaggerVersion = "v1";

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
      policy.SetIsOriginAllowed(origin =>
      {
        var host = new Uri(origin).Host;
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
builder.Services.AddDbContext<PositionContext>(options =>
{
  options.UseNpgsql(connectionString, npgsqlOptions => npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

// Setup HealthChecks
builder.Services.AddHealthChecks().AddDbContextCheck<PositionContext>();

// Setup Automapper
builder.Services.AddAutoMapper(cfg => { }, typeof(BaseballPositionProfile));

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
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc(SwaggerVersion, new OpenApiInfo { Title = SwaggerTitle, Version = SwaggerVersion });
  var currentAssembly = Assembly.GetExecutingAssembly();
  currentAssembly.GetReferencedAssemblies()
    .Union([currentAssembly.GetName()])
    .Select(assembly => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{assembly.Name}.xml"))
    .Where(filePath => File.Exists(filePath))
    .ToList()
    .ForEach(filePath => options.IncludeXmlComments(filePath));
});

// Setup Controllers
builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

// Build the App
var app = builder.Build();
app.UseCors(BaseballSpecificOrigins);
app.UseHsts();
app.UseRouting();
app.MapControllers();
app.MapHealthChecks("/api/health", new HealthCheckOptions { AllowCachingResponses = false });
app.UseSwagger(c => c.RouteTemplate = SwaggerBasePath + "/swagger/{documentName}/swagger.json");
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint($"/{SwaggerBasePath}/swagger/{SwaggerVersion}/swagger.json", $"{SwaggerTitle} - {SwaggerVersion}");
  c.RoutePrefix = $"{SwaggerBasePath}/swagger";
});

// Migrate Database on Startup
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<PositionContext>().Database.MigrateAsync();

// Start the App
await app.RunAsync();