using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infra.Data.Context
{
    public class TransportadorContextFactory : IDesignTimeDbContextFactory<TransportadorContext>
    {
        public TransportadorContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TransportadorContext>();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RGRTRASPORTE"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("RGRTRASPORTE");
            optionsBuilder.UseNpgsql(connectionString);

            return new TransportadorContext(optionsBuilder.Options);
        }
    }
} 
