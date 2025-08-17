using Dominio.Exceptions;
using Dominio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class DinheiroTests
    {
        [Theory]
        [InlineData(100.50, "BRL")]
        [InlineData(50.75, "USD")]
        [InlineData(25.25, "EUR")]
        public void Dinheiro_DeveCriarComValoresValidos(decimal valor, string moeda)
        {
            // Arrange & Act
            var dinheiro = new Dinheiro(valor, moeda);

            // Assert
            dinheiro.Valor.Should().Be(valor);
            dinheiro.Moeda.Should().Be(moeda.ToUpper());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100.50)]
        public void Dinheiro_DeveLancarExcecao_QuandoValorNegativo(decimal valor)
        {
            // Arrange & Act & Assert
            Action act = () => new Dinheiro(valor);
            act.Should().Throw<DomainException>()
                .WithMessage("Valor monetário não pode ser negativo");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Dinheiro_DeveLancarExcecao_QuandoMoedaInvalida(string moeda)
        {
            // Arrange & Act & Assert
            Action act = () => new Dinheiro(100, moeda);
            act.Should().Throw<DomainException>()
                .WithMessage("Código da moeda é obrigatório");
        }

        [Theory]
        [InlineData("REAL")] // 4 caracteres
        [InlineData("US")] // 2 caracteres
        public void Dinheiro_DeveLancarExcecao_QuandoMoedaTamanhoIncorreto(string moeda)
        {
            // Arrange & Act & Assert
            Action act = () => new Dinheiro(100, moeda);
            act.Should().Throw<DomainException>()
                .WithMessage("Código da moeda deve ter 3 caracteres");
        }

        [Theory]
        [InlineData("XYZ")]
        [InlineData("ABC")]
        public void Dinheiro_DeveLancarExcecao_QuandoMoedaNaoSuportada(string moeda)
        {
            // Arrange & Act & Assert
            Action act = () => new Dinheiro(100, moeda);
            act.Should().Throw<DomainException>()
                .WithMessage($"Moeda '{moeda}' não é suportada");
        }

        [Fact]
        public void Dinheiro_DeveSomarValoresMemsaMoeda()
        {
            // Arrange
            var dinheiro1 = new Dinheiro(100, "BRL");
            var dinheiro2 = new Dinheiro(50, "BRL");

            // Act
            var resultado = dinheiro1.Somar(dinheiro2);

            // Assert
            resultado.Valor.Should().Be(150);
            resultado.Moeda.Should().Be("BRL");
        }

        [Fact]
        public void Dinheiro_DeveLancarExcecao_QuandoSomarMoedasDiferentes()
        {
            // Arrange
            var dinheiro1 = new Dinheiro(100, "BRL");
            var dinheiro2 = new Dinheiro(50, "USD");

            // Act & Assert
            Action act = () => dinheiro1.Somar(dinheiro2);
            act.Should().Throw<DomainException>()
                .WithMessage("Não é possível operar com moedas diferentes: BRL e USD");
        }

        [Fact]
        public void Dinheiro_DeveSubtrairValores()
        {
            // Arrange
            var dinheiro1 = new Dinheiro(100, "BRL");
            var dinheiro2 = new Dinheiro(30, "BRL");

            // Act
            var resultado = dinheiro1.Subtrair(dinheiro2);

            // Assert
            resultado.Valor.Should().Be(70);
            resultado.Moeda.Should().Be("BRL");
        }

        [Fact]
        public void Dinheiro_DeveLancarExcecao_QuandoSubtracaoResultaNegativo()
        {
            // Arrange
            var dinheiro1 = new Dinheiro(50, "BRL");
            var dinheiro2 = new Dinheiro(100, "BRL");

            // Act & Assert
            Action act = () => dinheiro1.Subtrair(dinheiro2);
            act.Should().Throw<DomainException>()
                .WithMessage("Resultado da subtração não pode ser negativo");
        }

        [Theory]
        [InlineData(100, 2, 200)]
        [InlineData(50.50, 1.5, 75.75)]
        public void Dinheiro_DeveMultiplicarPorFator(decimal valor, decimal fator, decimal esperado)
        {
            // Arrange
            var dinheiro = new Dinheiro(valor, "BRL");

            // Act
            var resultado = dinheiro.Multiplicar(fator);

            // Assert
            resultado.Valor.Should().Be(esperado);
        }

        [Theory]
        [InlineData(100, 10)]
        [InlineData(100, 25)]
        [InlineData(100, 50)]
        public void Dinheiro_DeveAplicarDesconto(decimal valor, decimal percentual)
        {
            // Arrange
            var dinheiro = new Dinheiro(valor, "BRL");
            var valorEsperado = valor - (valor * percentual / 100);

            // Act
            var resultado = dinheiro.AplicarDesconto(percentual);

            // Assert
            resultado.Valor.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData(100, 10)]
        [InlineData(100, 25)]
        [InlineData(100, 50)]
        public void Dinheiro_DeveAplicarAcrescimo(decimal valor, decimal percentual)
        {
            // Arrange
            var dinheiro = new Dinheiro(valor, "BRL");
            var valorEsperado = valor + (valor * percentual / 100);

            // Act
            var resultado = dinheiro.AplicarAcrescimo(percentual);

            // Assert
            resultado.Valor.Should().Be(valorEsperado);
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(100, false)]
        public void Dinheiro_DeveVerificarSeEhZero(decimal valor, bool esperado)
        {
            // Arrange
            var dinheiro = new Dinheiro(valor, "BRL");

            // Act & Assert
            dinheiro.EhZero.Should().Be(esperado);
        }

        [Fact]
        public void Dinheiro_DeveUsarOperadores()
        {
            // Arrange
            var dinheiro1 = new Dinheiro(100, "BRL");
            var dinheiro2 = new Dinheiro(50, "BRL");

            // Act & Assert
            (dinheiro1 + dinheiro2).Valor.Should().Be(150);
            (dinheiro1 - dinheiro2).Valor.Should().Be(50);
            (dinheiro1 * 2).Valor.Should().Be(200);
            (dinheiro1 / 2).Valor.Should().Be(50);
            (dinheiro1 > dinheiro2).Should().BeTrue();
            (dinheiro1 < dinheiro2).Should().BeFalse();
        }

        [Theory]
        [InlineData(100.50, "BRL", "R$ 100,50")]
        [InlineData(100.50, "USD", "$100.50")]
        [InlineData(100.50, "EUR", "100,50 €")]
        public void Dinheiro_DeveFormatarPorMoeda(decimal valor, string moeda, string formatoEsperado)
        {
            // Arrange
            var dinheiro = new Dinheiro(valor, moeda);

            // Act
            var formato = dinheiro.ValorFormatado;

            // Assert
            formato.Should().Be(formatoEsperado);
            // Nota: O teste pode variar dependendo da cultura do sistema
        }

        [Fact]
        public void Dinheiro_DeveUsarFactoryMethods()
        {
            // Act
            var real = Dinheiro.Real(100);
            var dolar = Dinheiro.Dolar(100);
            var euro = Dinheiro.Euro(100);
            var zero = Dinheiro.Zero();

            // Assert
            real.Moeda.Should().Be("BRL");
            dolar.Moeda.Should().Be("USD");
            euro.Moeda.Should().Be("EUR");
            zero.Valor.Should().Be(0);
            zero.Moeda.Should().Be("BRL");
        }
    }
}
