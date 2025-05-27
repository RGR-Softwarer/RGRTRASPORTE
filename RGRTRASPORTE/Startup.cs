using Hangfire;
using Hangfire.PostgreSql;
using Infra.Ioc;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Infra.Ioc.HealthChecks;
using Prometheus;

namespace RGRTRASPORTE
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

            services.AddRouting();
            services.AddControllers();

            services.AddSingleton(_configuration);

            services.AddContext(_configuration);

            services.AddSwagger();

            services.ConfigureApi();

            services.AddServices();

            services.AddRepositories();

            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Viagem.AdicionarViagemCommand).Assembly);
            });

            var connectionString = _configuration.GetConnectionString("RGRTRASPORTE") ?? string.Empty;

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpMetrics();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RGR Transporte v1"));
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapMetrics();

                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (_) => false,
                    ResponseWriter = HealthCheckExtensions.WriteResponse
                });
            });
        }
    }
}
