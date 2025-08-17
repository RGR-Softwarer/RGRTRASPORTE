using Dominio.Exceptions;
using Dominio.ValueObjects;
using Dominio.Enums.Pessoas;
using Dominio.Enums.Veiculo;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class CNHTests
    {
        [Theory]
        [InlineData("12345678901", CategoriaCNHEnum.B)]
        [InlineData("98765432109", CategoriaCNHEnum.D)]
        public void CNH_DeveCriarComDadosValidos(string numero, CategoriaCNHEnum categoria)
        {
            // Arrange & Act
            var validade = DateTime.Today.AddYears(5);
            var cnh = new CNH(numero, categoria, validade);

            // Assert
            cnh.Numero.Should().Be(numero);
            cnh.Categoria.Should().Be(categoria);
            cnh.Validade.Should().Be(validade);
            cnh.Expirada.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CNH_DeveLancarExcecao_QuandoNumeroInvalido(string numero)
        {
            // Arrange & Act & Assert
            Action act = () => new CNH(numero, CategoriaCNHEnum.B, DateTime.Today.AddYears(1));
            act.Should().Throw<DomainException>()
                .WithMessage("Número da CNH é obrigatório");
        }

        [Theory]
        [InlineData("123456789")] // 9 dígitos
        [InlineData("1234567890123")] // 13 dígitos
        public void CNH_DeveLancarExcecao_QuandoTamanhoIncorreto(string numero)
        {
            // Arrange & Act & Assert
            Action act = () => new CNH(numero, CategoriaCNHEnum.B, DateTime.Today.AddYears(1));
            act.Should().Throw<DomainException>()
                .WithMessage("CNH deve ter 11 dígitos");
        }

        [Theory]
        [InlineData("1234567890a")] // Contém letra
        [InlineData("123456789@1")] // Contém símbolo
        public void CNH_DeveLancarExcecao_QuandoContemCaracteresInvalidos(string numero)
        {
            // Arrange & Act & Assert
            Action act = () => new CNH(numero, CategoriaCNHEnum.B, DateTime.Today.AddYears(1));
            act.Should().Throw<DomainException>()
                .WithMessage("CNH deve conter apenas números");
        }

        [Fact]
        public void CNH_DeveLancarExcecao_QuandoValidadeAnterior()
        {
            // Arrange & Act & Assert
            Action act = () => new CNH("12345678901", CategoriaCNHEnum.B, DateTime.Today.AddDays(-1));
            act.Should().Throw<DomainException>()
                .WithMessage("Data de validade da CNH não pode ser anterior à data atual");
        }

        [Fact]
        public void CNH_DeveDetectarCNHExpirada()
        {
            // Arrange
            var cnh = new CNH("12345678901", CategoriaCNHEnum.B, DateTime.Today.AddDays(-1));

            // Act & Assert
            cnh.Expirada.Should().BeTrue();
        }

        [Theory]
        [InlineData(TipoModeloVeiculoEnum.Carro, CategoriaCNHEnum.B, true)]
        [InlineData(TipoModeloVeiculoEnum.Van, CategoriaCNHEnum.B, true)]
        [InlineData(TipoModeloVeiculoEnum.Van, CategoriaCNHEnum.D, true)]
        [InlineData(TipoModeloVeiculoEnum.Onibus, CategoriaCNHEnum.D, true)]
        [InlineData(TipoModeloVeiculoEnum.Onibus, CategoriaCNHEnum.B, false)]
        public void CNH_DeveVerificarHabilitacaoParaTipoVeiculo(TipoModeloVeiculoEnum tipoVeiculo, CategoriaCNHEnum categoria, bool podeConduzir)
        {
            // Arrange
            var cnh = new CNH("12345678901", categoria, DateTime.Today.AddYears(1));

            // Act
            var resultado = cnh.PodeConduzirTipoVeiculo(tipoVeiculo);

            // Assert
            resultado.Should().Be(podeConduzir);
        }

        [Fact]
        public void CNH_DeveRenovarComNovaValidade()
        {
            // Arrange
            var cnhOriginal = new CNH("12345678901", CategoriaCNHEnum.B, DateTime.Today.AddYears(1));
            var novaValidade = DateTime.Today.AddYears(5);

            // Act
            var cnhRenovada = cnhOriginal.RenovarCNH(novaValidade);

            // Assert
            cnhRenovada.Numero.Should().Be(cnhOriginal.Numero);
            cnhRenovada.Categoria.Should().Be(cnhOriginal.Categoria);
            cnhRenovada.Validade.Should().Be(novaValidade);
        }

        [Fact]
        public void CNH_DeveFormatarNumeroCorretamente()
        {
            // Arrange
            var cnh = new CNH("12345678901", CategoriaCNHEnum.B, DateTime.Today.AddYears(1));

            // Act
            var numeroFormatado = cnh.NumeroFormatado;

            // Assert
            numeroFormatado.Should().Be("123.456.789-01");
        }

        [Fact]
        public void CNH_DeveCalcularDiasParaVencer()
        {
            // Arrange
            var diasParaVencer = 30;
            var validade = DateTime.Today.AddDays(diasParaVencer);
            var cnh = new CNH("12345678901", CategoriaCNHEnum.B, validade);

            // Act
            var resultado = cnh.DiasParaVencer;

            // Assert
            resultado.Should().Be(diasParaVencer);
        }

        [Fact]
        public void CNH_DeveDetectarVencimentoEm30Dias()
        {
            // Arrange
            var cnh = new CNH("12345678901", CategoriaCNHEnum.B, DateTime.Today.AddDays(30));

            // Act & Assert
            cnh.VencendoEm30Dias.Should().BeTrue();
        }
    }
}
