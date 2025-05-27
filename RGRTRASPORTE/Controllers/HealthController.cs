using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace RGRTRASPORTE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        [Route("/health")]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            var result = JsonSerializer.Serialize(new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.ToString()
                })
            });

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(503, result);
        }

        [HttpGet]
        [Route("/health/ready")]
        public async Task<IActionResult> ReadyCheck()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            
            return report.Status == HealthStatus.Healthy
                ? Ok("Aplicação está pronta")
                : StatusCode(503, "Aplicação não está pronta");
        }

        [HttpGet]
        [Route("/health/live")]
        public IActionResult LiveCheck()
        {
            return Ok("Aplicação está em execução");
        }
    }
} 