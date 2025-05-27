using Microsoft.Extensions.Diagnostics.HealthChecks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using Infra.Ioc.HealthChecks;

namespace Teste.HealthChecks
{
    public class HealthCheckTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public HealthCheckTests()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    // Adiciona serviço de roteamento
                    services.AddRouting();

                    // Adiciona controllers
                    services.AddControllers();

                    // Configura Health Checks
                    services.AddHealthChecks()
                        .AddCheck("test_check", () => 
                            HealthCheckResult.Healthy("Sistema está saudável"));
                })
                .Configure(app =>
                {
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        
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
                });

            _server = new TestServer(webHostBuilder);
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_DeveRetornarHealthy()
        {
            // Act
            var response = await _client.GetAsync("/health");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonDocument>(content);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Healthy", result.RootElement.GetProperty("status").GetString());
        }

        [Fact]
        public async Task HealthCheck_Ready_DeveRetornarOk()
        {
            // Act
            var response = await _client.GetAsync("/health/ready");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_Live_DeveRetornarOk()
        {
            // Act
            var response = await _client.GetAsync("/health/live");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_EndpointInvalido_DeveRetornar404()
        {
            // Act
            var response = await _client.GetAsync("/health-invalido");
            
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
} 