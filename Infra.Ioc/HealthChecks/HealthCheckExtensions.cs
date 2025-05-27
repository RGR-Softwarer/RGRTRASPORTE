using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text;
using System.Text.Json;

namespace Infra.Ioc.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddHealthChecks()
                // Verifica o schema public
                .AddNpgSql(
                    connectionString,
                    name: "database-public",
                    tags: new[] { "db", "sql", "postgresql" },
                    timeout: TimeSpan.FromSeconds(5),
                    healthQuery: "SELECT 1 FROM pg_database WHERE datname = current_database()")

                // Verifica o schema hangfire
                .AddNpgSql(
                    connectionString,
                    name: "database-hangfire",
                    tags: new[] { "db", "sql", "postgresql", "hangfire" },
                    timeout: TimeSpan.FromSeconds(5),
                    healthQuery: "SELECT 1 FROM pg_tables WHERE schemaname = 'hangfire'")

                // Verifica o serviço Hangfire
                .AddHangfire(
                    options =>
                    {
                        options.MinimumAvailableServers = 1;
                        options.MaximumJobsFailed = 5;
                    },
                    name: "hangfire-service",
                    tags: new[] { "jobs", "tasks", "background" })

                // Verifica o uso de memória
                .AddCheck<ProcessHealthCheck>(
                    name: "memory",
                    tags: new[] { "memory", "process" })

                // Verifica o tempo de atividade
                .AddCheck<UptimeHealthCheck>(
                    name: "uptime",
                    tags: new[] { "system" });

            return services;
        }

        public static Task WriteResponse(
            HttpContext context,
            HealthReport healthReport)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions { Indented = true };

            using var memoryStream = new MemoryStream();
            using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
            {
                jsonWriter.WriteStartObject();
                jsonWriter.WriteString("status", healthReport.Status.ToString());
                jsonWriter.WriteStartObject("results");

                foreach (var healthReportEntry in healthReport.Entries)
                {
                    jsonWriter.WriteStartObject(healthReportEntry.Key);
                    jsonWriter.WriteString("status", healthReportEntry.Value.Status.ToString());
                    jsonWriter.WriteString("description", healthReportEntry.Value.Description);
                    jsonWriter.WriteStartObject("data");

                    foreach (var item in healthReportEntry.Value.Data)
                    {
                        jsonWriter.WritePropertyName(item.Key);
                        JsonSerializer.Serialize(
                            jsonWriter, item.Value,
                            item.Value?.GetType() ?? typeof(object));
                    }

                    jsonWriter.WriteEndObject();
                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndObject();
                jsonWriter.WriteEndObject();
            }

            return context.Response.WriteAsync(
                Encoding.UTF8.GetString(memoryStream.ToArray()));
        }
    }

    public class ProcessHealthCheck : IHealthCheck
    {
        private const long MaximumMemoryBytes = 1024L * 1024L * 1024L; // 1GB

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var memoryUsage = currentProcess.WorkingSet64;

            var status = memoryUsage < MaximumMemoryBytes
                ? HealthStatus.Healthy
                : HealthStatus.Degraded;

            var data = new Dictionary<string, object>
            {
                { "MemoryUsage", memoryUsage },
                { "MemoryLimit", MaximumMemoryBytes },
                { "ProcessId", currentProcess.Id },
                { "ProcessName", currentProcess.ProcessName }
            };

            return Task.FromResult(new HealthCheckResult(
                status,
                description: $"Processo está usando {memoryUsage / 1024 / 1024}MB de memória",
                data: data));
        }
    }

    public class UptimeHealthCheck : IHealthCheck
    {
        private readonly DateTime _startTime = DateTime.UtcNow;

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var uptime = DateTime.UtcNow - _startTime;

            var data = new Dictionary<string, object>
            {
                { "StartTime", _startTime },
                { "Uptime", uptime.ToString() }
            };

            return Task.FromResult(new HealthCheckResult(
                HealthStatus.Healthy,
                description: $"Sistema está rodando há {uptime.Days} dias, {uptime.Hours} horas e {uptime.Minutes} minutos",
                data: data));
        }
    }
}