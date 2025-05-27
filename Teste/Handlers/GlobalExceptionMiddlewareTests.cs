using Infra.CrossCutting.Exceptions;
using Infra.CrossCutting.Handlers.ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Teste.Handlers
{
    public class GlobalExceptionMiddlewareTests
    {
        private readonly Mock<ILogger<GlobalExceptionMiddleware>> _loggerMock;
        private readonly DefaultHttpContext _context;

        public GlobalExceptionMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<GlobalExceptionMiddleware>>();
            _context = new DefaultHttpContext();
            _context.Response.Body = new MemoryStream();
        }

        private GlobalExceptionMiddleware CreateMiddleware(Exception exception)
        {
            return new GlobalExceptionMiddleware(
                next: (innerHttpContext) => Task.FromException(exception),
                _loggerMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_WithBusinessException_ReturnsCorrectResponse()
        {
            // Arrange
            var middleware = CreateMiddleware(
                new BusinessException("Business error", HttpStatusCode.BadRequest));

            // Act
            await middleware.InvokeAsync(_context);

            // Assert
            _context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(_context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

            Assert.Equal((int)HttpStatusCode.BadRequest, _context.Response.StatusCode);
            Assert.Equal("Business error", response.Message);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task InvokeAsync_WithKeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            var middleware = CreateMiddleware(
                new KeyNotFoundException("Not found"));

            // Act
            await middleware.InvokeAsync(_context);

            // Assert
            _context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(_context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ErrorResponse>(responseBody);

            Assert.Equal((int)HttpStatusCode.NotFound, _context.Response.StatusCode);
            Assert.Equal("Recurso não encontrado.", response.Details);
            Assert.False(response.Success);
        }
    }
} 