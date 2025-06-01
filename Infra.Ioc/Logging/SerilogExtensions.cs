using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Infra.Ioc.Logging
{
    public static class SerilogExtensions
    {
        public static IHostBuilder AddSerilog(
            this IHostBuilder builder,
            IConfiguration configuration)
        {
            var elasticUri = configuration["Elasticsearch:Uri"];
            var applicationName = configuration["ApplicationName"] ?? "RGRTransporte";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(new ElasticsearchJsonFormatter())
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = $"{applicationName.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                    MinimumLogEventLevel = LogEventLevel.Information,
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                     EmitEventFailureHandling.WriteToFailureSink |
                                     EmitEventFailureHandling.RaiseCallback
                })
                .WriteTo.File(
                    path: $"logs/{applicationName}-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    fileSizeLimitBytes: 5242880) // 5MB
                .CreateLogger();

            builder.UseSerilog();

            return builder;
        }

        public static IServiceCollection AddSerilogLogging(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var elasticUri = configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";
            var applicationName = configuration["ApplicationName"] ?? "RGRTransporte";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(
                    path: $"logs/{applicationName}-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    fileSizeLimitBytes: 5242880) // 5MB
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            return services;
        }
    }
} 