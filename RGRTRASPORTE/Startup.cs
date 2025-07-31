using Hangfire;
using Hangfire.PostgreSql;
using Infra.Ioc;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Infra.Ioc.HealthChecks;
using Prometheus;
using AutoMapper;

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
            services.AddRouting();
            
            // Configurar controladores para aceitar enums como strings
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configurar enums para serem serializados/deserializados como strings
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                    
                    // Opcional: configurar para ser case-insensitive
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            services.AddSingleton(_configuration);

            services.AddContext(_configuration);

            services.AddSwagger();

            services.ConfigureApi();

            services.AddInfrastructure(_configuration);

            // Auditoria
            services.AddAuditoria();

            // AutoMapper
            services.AddAutoMapper(typeof(Startup), typeof(Application.Commands.Viagem.IniciarViagemCommand));

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
