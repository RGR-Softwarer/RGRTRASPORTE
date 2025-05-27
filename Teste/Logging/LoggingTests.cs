using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Teste.Logging
{
    public class LoggingTests : IDisposable
    {
        private readonly ILogger _logger;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;

        public LoggingTests()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            var loggerMock = new Mock<ILogger>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            _logger = loggerMock.Object;
        }

        public void Dispose()
        {
            // Limpa os logs após cada teste
            Log.CloseAndFlush();
        }

        [Fact]
        public void TestarLogInformacao()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.TestCorrelator()
                    .CreateLogger();

                // Act
                logger.Information("Teste de log de informação");

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.Single(logEvents);
                Assert.Equal(LogEventLevel.Information, logEvents.First().Level);
                Assert.Equal("Teste de log de informação", logEvents.First().MessageTemplate.Text);
            }
        }

        [Fact]
        public void TestarLogErro()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.TestCorrelator()
                    .CreateLogger();

                var exception = new Exception("Erro de teste");

                // Act
                logger.Error(exception, "Ocorreu um erro: {ErrorMessage}", exception.Message);

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.Single(logEvents);
                Assert.Equal(LogEventLevel.Error, logEvents.First().Level);
                Assert.Contains("Ocorreu um erro", logEvents.First().MessageTemplate.Text);
                Assert.NotNull(logEvents.First().Exception);
            }
        }

        [Fact]
        public void TestarLogWarning()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.TestCorrelator()
                    .CreateLogger();

                // Act
                logger.Warning("Aviso importante: {Mensagem}", "Teste de warning");

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.Single(logEvents);
                Assert.Equal(LogEventLevel.Warning, logEvents.First().Level);
                Assert.Contains("Aviso importante", logEvents.First().MessageTemplate.Text);
            }
        }

        [Fact]
        public void TestarLogComContexto()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.TestCorrelator()
                    .Enrich.WithProperty("Contexto", "TesteUnidade")
                    .CreateLogger();

                // Act
                logger.Information("Log com contexto");

                // Assert
                var logEvents = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.Single(logEvents);
                Assert.Equal("TesteUnidade", logEvents.First().Properties["Contexto"].ToString().Trim('"'));
            }
        }
    }
} 