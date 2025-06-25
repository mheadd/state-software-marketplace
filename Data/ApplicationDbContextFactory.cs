using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace state_software_marketplace.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Log current directory and environment for debugging
            var currentDirectory = Directory.GetCurrentDirectory();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            
            Console.WriteLine($"[Factory] Current Directory: {currentDirectory}");
            Console.WriteLine($"[Factory] Environment: {environment}");

            // Build configuration, looking in multiple locations
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables();

            if (args != null && args.Length > 0)
            {
                configBuilder.AddCommandLine(args);
            }

            var configuration = configBuilder.Build();

            // First try environment variable
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

            // Then try configuration if not found in environment
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            Console.WriteLine($"[Factory] Connection String Found: {!string.IsNullOrEmpty(connectionString)}");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                var error = "No connection string found. Ensure either DB_CONNECTION_STRING environment variable or DefaultConnection in appsettings.json is set.";
                Console.WriteLine($"[Factory] Error: {error}");
                throw new InvalidOperationException(error);
            }

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            if (environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
