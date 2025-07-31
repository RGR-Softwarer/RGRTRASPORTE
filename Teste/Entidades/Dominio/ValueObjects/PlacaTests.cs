using Dominio.Exceptions;
using Dominio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class PlacaTests
    {
        [Theory]
        [InlineData("ABC1234")] // Formato antigo
        [InlineData("ABC1D23")] // Formato Mercosul
        [InlineData("XYZ9876")]
        public void Placa_DeveCriarComPlacaValida(string numeroPlaca)
        {
            // Act
            var placa = new Placa(numeroPlaca);

            // Assert
            placa.Numero.Should().Be(numeroPlaca.ToUpper());
        }

        [Theory]
        [InlineData("abc1234")] // Minúscula
        [InlineData("xyz9876")] // Minúscula
        public void Placa_DeveConverterParaMaiuscula(string numeroPlaca)
        {
            // Act
            var placa = new Placa(numeroPlaca);

            // Assert
            placa.Numero.Should().Be(numeroPlaca.ToUpper());
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Placa_DeveLancarExcecao_QuandoPlacaVaziaOuNula(string numeroPlaca)
        {
            // Act & Assert
            Action act = () => new Placa(numeroPlaca);
            act.Should().Throw<DomainException>()
                .WithMessage("Placa é obrigatória.");
        }

        [Theory]
        [InlineData("ABC123")] // 6 caracteres
        [InlineData("ABC12345")] // 8 caracteres
        [InlineData("AB1234")] // 6 caracteres
        public void Placa_DeveLancarExcecao_QuandoTamanhoInvalido(string numeroPlaca)
        {
            // Act & Assert
            Action act = () => new Placa(numeroPlaca);
            act.Should().Throw<DomainException>()
                .WithMessage("Placa deve ter 7 caracteres.");
        }

        [Theory]
        [InlineData("ABC@123")] // Símbolo especial
        [InlineData("ABC-123")] // Hífen
        [InlineData("ABC 123")] // Espaço
        public void Placa_DeveLancarExcecao_QuandoCaracteresInvalidos(string numeroPlaca)
        {
            // Act & Assert
            Action act = () => new Placa(numeroPlaca);
            act.Should().Throw<DomainException>()
                .WithMessage("Placa deve conter apenas letras e números.");
        }

        [Theory]
        [InlineData("ABC1234", "ABC-1234")]
        [InlineData("XYZ9876", "XYZ-9876")]
        public void Formatada_DeveRetornarPlacaComHifen(string numeroPlaca, string esperado)
        {
            // Arrange
            var placa = new Placa(numeroPlaca);

            // Act
            var formatado = placa.Formatada;

            // Assert
            formatado.Should().Be(esperado);
        }

        [Fact]
        public void Placa_DeveSerIgual_QuandoNumerosIguais()
        {
            // Arrange
            var placa1 = new Placa("ABC1234");
            var placa2 = new Placa("abc1234"); // Case insensitive

            // Act & Assert
            placa1.Should().Be(placa2);
            (placa1.Numero == placa2.Numero).Should().BeTrue();
            (placa1.Numero != placa2.Numero).Should().BeFalse();
        }

        [Fact]
        public void Placa_DeveSerDiferente_QuandoNumerosDiferentes()
        {
            // Arrange
            var placa1 = new Placa("ABC1234");
            var placa2 = new Placa("XYZ9876");

            // Act & Assert
            placa1.Should().NotBe(placa2);
            (placa1 == placa2).Should().BeFalse();
            (placa1 != placa2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_DeveRetornarHashIgual_ParaPlacasIguais()
        {
            // Arrange
            var placa1 = new Placa("ABC1234");
            var placa2 = new Placa("abc1234");

            // Act & Assert
            placa1.GetHashCode().Should().Be(placa2.GetHashCode());
        }

        [Fact]
        public void ToString_DeveRetornarPlacaFormatada()
        {
            // Arrange
            var placa = new Placa("ABC1234");

            // Act
            var resultado = placa.ToString();

            // Assert
            resultado.Should().Be("ABC-1234");
        }

        [Fact]
        public void OperadorImplicito_DeveConverterStringParaPlaca()
        {
            // Act
            Placa placa = "ABC1234";

            // Assert
            placa.Numero.Should().Be("ABC1234");
        }

        [Fact]
        public void OperadorImplicito_DeveConverterPlacaParaString()
        {
            // Arrange
            var placa = new Placa("ABC1234");

            // Act
            string numero = placa;

            // Assert
            numero.Should().Be("ABC1234");
        }
    }
}