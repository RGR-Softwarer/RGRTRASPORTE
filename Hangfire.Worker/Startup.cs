using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Infra.Ioc;

namespace Hangfire.Worker
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            var connectionString = _configuration.GetConnectionString("RGRTRASPORTE") ?? string.Empty;

            services.AddContext(_configuration);
            services.AddInfrastructure(_configuration);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddConsole();
            });

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UsePostgreSqlStorage(options =>
                      {
                          options.UseNpgsqlConnection(connectionString);
                      });
            });

            services.AddHangfireServer(options =>
            {
                options.ServerName = "Hangfire.Postgres";
                options.Queues = new[] { "default" };
            });

           // services.AddScoped<ITesteJob, TesteJob>();
        }

        public Task Configure(WebApplication app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs = null)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Hangfire Dashboard",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = _configuration["HangfireCredentials:UserName"] ?? "",
                        Pass = _configuration["HangfireCredentials:Password"] ?? ""
                    }
                }
            });

            app.MapHangfireDashboard();

            // Hangfire server j� configurado no ConfigureServices com AddHangfireServer

            var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

            //recurringJobManager.AddOrUpdate<ITesteJob>(
            //    "CapturarDados",
            //    job => job.ExecutarAsync(),
            //    "0 8-23 * * *");

            return Task.CompletedTask;
        }
    }
}
