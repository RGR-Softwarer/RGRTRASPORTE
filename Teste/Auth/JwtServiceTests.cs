using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infra.CrossCutting.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Teste.Auth
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly JwtSettings _jwtSettings;

        public JwtServiceTests()
        {
            _jwtSettings = new JwtSettings
            {
                SecretKey = "Sua_Chave_Secreta_Para_Testes_Com_Mais_De_32_Caracteres",
                Issuer = "SeuIssuer",
                Audience = "SuaAudience",
                ExpirationInMinutes = 60
            };

            var options = Options.Create(_jwtSettings);
            _jwtService = new JwtService(options);
        }

        [Fact]
        public void GenerateToken_DeveRetornarTokenValido()
        {
            // Arrange
            var userId = "123";
            var userName = "usuario.teste";
            var roles = new[] { "Admin" };

            // Act
            var token = _jwtService.GenerateToken(userId, userName, roles);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId);
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Name && c.Value == userName);
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == "Admin");
        }

        [Fact]
        public void GenerateToken_DeveTerTempoExpiracaoCorreto()
        {
            // Arrange
            var userId = "123";
            var userName = "usuario.teste";
            var roles = new[] { "User" };

            // Act
            var token = _jwtService.GenerateToken(userId, userName, roles);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
            Assert.True(jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(61)); // 60 minutos + 1 de margem
        }

        [Fact]
        public void GenerateToken_DeveConterInformacoesCorretas()
        {
            // Arrange
            var userId = "123";
            var userName = "usuario.teste";
            var roles = new[] { "User" };

            // Act
            var token = _jwtService.GenerateToken(userId, userName, roles);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert
            Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
            Assert.Equal(_jwtSettings.Audience, jwtToken.Audiences.First());
        }

        [Fact]
        public void ValidateToken_DeveRetornarClaimsPrincipal()
        {
            // Arrange
            var userId = "123";
            var userName = "usuario.teste";
            var roles = new[] { "Admin" };
            var token = _jwtService.GenerateToken(userId, userName, roles);

            // Act
            var principal = _jwtService.ValidateToken(token);

            // Assert
            Assert.NotNull(principal);
            Assert.Equal(userName, principal.FindFirst(JwtRegisteredClaimNames.Name)?.Value);
            Assert.True(principal.IsInRole("Admin"));
        }

        [Fact]
        public void ValidateToken_TokenInvalido_DeveRetornarNull()
        {
            // Arrange
            var tokenInvalido = "token_invalido";

            // Act
            var principal = _jwtService.ValidateToken(tokenInvalido);

            // Assert
            Assert.Null(principal);
        }
    }
} 
