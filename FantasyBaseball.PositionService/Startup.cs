using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FantasyBaseball.PositionService.Database;
using FantasyBaseball.PositionService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FantasyBaseball.PositionService
{
    /// <summary>The class that sets up all of the configuration for the service.</summary>
    public class Startup
    {
        private const string SwaggerBasePath = "api/v1/position";
        private const string SwaggerTitle = "FantasyBaseball.PositionService";
        private const string SwaggerVersion = "v1";

        /// <summary>Creates a new instance of the startup and sets up the configuration object.</summary>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public Startup(IHostEnvironment env) =>
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

        /// <summary>Represents a set of key/value application configuration properties.</summary>
        public IConfiguration Configuration { get; }

        /// <summary>This method configures the HTTP request pipeline.</summary>
        /// <param name="app">The object to convert to a string.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IHostEnvironment env) 
        {
            app
                .UseCors()
                .UseHsts()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers())
                .UseEndpoints(endpoints => endpoints.MapHealthChecks("/api/health"))
                .UseSwagger(c => c.RouteTemplate = SwaggerBasePath + "/swagger/{documentName}/swagger.json")
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/{SwaggerBasePath}/swagger/v1/swagger.json", $"{SwaggerTitle} - {SwaggerVersion}");
                    c.RoutePrefix = $"{SwaggerBasePath}/swagger";
                });
            using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PositionContext>().Database.Migrate();
        }

        /// <summary>This method adds the services to the container.</summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = string.Format(
                Configuration.GetConnectionString("PositionDatabase"), 
                Configuration["POSITION_DATABASE_HOST"], 
                Configuration["POSITION_DATABASE"],
                Configuration["POSITION_DATABASE_USER"], 
                Configuration["POSITION_DATABASE_PASSWORD"]);
            services.AddHealthChecks().AddDbContextCheck<PositionContext>();
            services
                .AddCors(options => options.AddDefaultPolicy(builder => builder
                    .SetIsOriginAllowed(IsValidHost)
                    .AllowAnyHeader()
                    .AllowAnyMethod()))
                .AddSingleton(Configuration)
                .AddDbContext<PositionContext>(options => options.UseNpgsql(connectionString))
                .AddScoped<IPositionContext>(provider => provider.GetService<PositionContext>())
                .AddSingleton<IBaseballPositionBuilderService, BaseballPositionBuilderService>()
                .AddScoped<IGetPositionsService, GetPositionsService>()
                .AddSingleton<ISortService, SortService>()
                .AddSwaggerGen(o => 
                {
                    o.SwaggerDoc(SwaggerVersion, new OpenApiInfo { Title = SwaggerTitle, Version = SwaggerVersion });
                    var currentAssembly = Assembly.GetExecutingAssembly();  
                    currentAssembly.GetReferencedAssemblies()  
                        .Union(new AssemblyName[] { currentAssembly.GetName() })  
                        .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location), $"{a.Name}.xml"))  
                        .Where(f => File.Exists(f))
                        .ToList()
                        .ForEach(f => o.IncludeXmlComments(f));  
                })
                .AddControllers();
        }

        private static bool IsValidHost(string origin)
        {
            var host = new Uri(origin).Host;
            return host == "localhost" || host.Contains("schultz.local");
        }
    }
}