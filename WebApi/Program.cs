using Infrastructure.Identity;
using Infrastructure.Identity.Contexts;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Seeds;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

                    // Apply migrations
                    var identityContext = services.GetRequiredService<IdentityContext>();
                    await identityContext.Database.MigrateAsync();

                    var appDbContext = services.GetRequiredService<ApplicationDbContext>();
                    await appDbContext.Database.MigrateAsync();

                    // Seed identity roles and users
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DefaultRoles.SeedAsync(userManager, roleManager);
                    await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                    await DefaultBasicUser.SeedAsync(userManager, roleManager);

                    Log.Information("✅ Database migration and seeding complete.");
                }

                Log.Information("🚀 Application starting...");
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "❌ Application terminated unexpectedly during startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog() // Use Serilog instead of built-in logging
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://0.0.0.0:80");

                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<OrderStatusUpdateBackgroundService>(); // background service
                });
    }
}
