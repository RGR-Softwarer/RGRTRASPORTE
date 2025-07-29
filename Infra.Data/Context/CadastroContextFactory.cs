using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infra.Data.Context
{
    public class CadastroContextFactory : IDesignTimeDbContextFactory<CadastroContext>
    {
        public CadastroContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CadastroContext>();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../RGRTRASPORTE"))
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("RGRTRASPORTE");
            optionsBuilder.UseNpgsql(connectionString);

            return new CadastroContext(optionsBuilder.Options);
        }
    }
} 