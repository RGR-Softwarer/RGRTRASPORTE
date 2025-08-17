using Dominio.Exceptions;
using Dominio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class CapacidadeVeiculoTests
    {
        [Theory]
        [InlineData(40, 10, 50)]
        [InlineData(20, 0, 20)]
        [InlineData(50, 20, 70)]
        public void CapacidadeVeiculo_DeveCriarComValoresValidos(int assentos, int emPe, int capacidadeEsperada)
        {
            // Arrange & Act
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Assert
            capacidade.AssentosDisponiveis.Should().Be(assentos);
            capacidade.PassageirosEmPe.Should().Be(emPe);
            capacidade.CapacidadeMaxima.Should().Be(capacidadeEsperada);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void CapacidadeVeiculo_DeveLancarExcecao_QuandoAssentosInvalidos(int assentos)
        {
            // Arrange & Act & Assert
            Action act = () => new CapacidadeVeiculo(assentos, 0);
            act.Should().Throw<DomainException>()
                .WithMessage("Quantidade de assentos deve ser maior que zero");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        public void CapacidadeVeiculo_DeveLancarExcecao_QuandoPassageirosEmPeNegativo(int emPe)
        {
            // Arrange & Act & Assert
            Action act = () => new CapacidadeVeiculo(20, emPe);
            act.Should().Throw<DomainException>()
                .WithMessage("Quantidade de passageiros em pé não pode ser negativa");
        }

        [Fact]
        public void CapacidadeVeiculo_DeveLancarExcecao_QuandoAssentosMuitoAltos()
        {
            // Arrange & Act & Assert
            Action act = () => new CapacidadeVeiculo(101, 0);
            act.Should().Throw<DomainException>()
                .WithMessage("Quantidade de assentos não pode ser maior que 100");
        }

        [Fact]
        public void CapacidadeVeiculo_DeveLancarExcecao_QuandoPassageirosEmPeMuitoAltos()
        {
            // Arrange & Act & Assert
            Action act = () => new CapacidadeVeiculo(20, 51);
            act.Should().Throw<DomainException>()
                .WithMessage("Quantidade de passageiros em pé não pode ser maior que 50");
        }

        [Fact]
        public void CapacidadeVeiculo_DeveLancarExcecao_QuandoCapacidadeTotalMuitoAlta()
        {
            // Arrange & Act & Assert
            Action act = () => new CapacidadeVeiculo(100, 51);
            act.Should().Throw<DomainException>()
                .WithMessage("Capacidade total não pode ser maior que 150 passageiros");
        }

        [Theory]
        [InlineData(40, 10, 30, true)]
        [InlineData(40, 10, 50, true)]
        [InlineData(40, 10, 51, false)]
        public void CapacidadeVeiculo_DeveVerificarSeTemCapacidadePara(int assentos, int emPe, int passageiros, bool temCapacidade)
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Act
            var resultado = capacidade.TemCapacidadePara(passageiros);

            // Assert
            resultado.Should().Be(temCapacidade);
        }

        [Theory]
        [InlineData(40, 10, 20, 30)]
        [InlineData(40, 10, 40, 10)]
        [InlineData(40, 10, 50, 0)]
        [InlineData(40, 10, 60, 0)] // Não pode ser negativo
        public void CapacidadeVeiculo_DeveCalcularVagasRemanescentes(int assentos, int emPe, int embarcados, int vagasEsperadas)
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Act
            var vagas = capacidade.VagasRemanescentesPara(embarcados);

            // Assert
            vagas.Should().Be(vagasEsperadas);
        }

        [Theory]
        [InlineData(40, 10, 30, false)]
        [InlineData(40, 10, 50, true)]
        [InlineData(40, 10, 60, true)] // Superlotado
        public void CapacidadeVeiculo_DeveVerificarSeLotado(int assentos, int emPe, int embarcados, bool estaLotado)
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Act
            var resultado = capacidade.EstaLotado(embarcados);

            // Assert
            resultado.Should().Be(estaLotado);
        }

        [Theory]
        [InlineData(40, 10, 25, 50.0)]
        [InlineData(40, 10, 50, 100.0)]
        [InlineData(40, 10, 0, 0.0)]
        public void CapacidadeVeiculo_DeveCalcularTaxaOcupacao(int assentos, int emPe, int embarcados, double taxaEsperada)
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Act
            var taxa = capacidade.TaxaOcupacao(embarcados);

            // Assert
            taxa.Should().BeApproximately(taxaEsperada, 0.1);
        }

        [Fact]
        public void CapacidadeVeiculo_DeveAtualizarAssentos()
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(40, 10);

            // Act
            var novaCapacidade = capacidade.AtualizarAssentos(50);

            // Assert
            novaCapacidade.AssentosDisponiveis.Should().Be(50);
            novaCapacidade.PassageirosEmPe.Should().Be(10);
            novaCapacidade.CapacidadeMaxima.Should().Be(60);
        }

        [Fact]
        public void CapacidadeVeiculo_DeveAtualizarPassageirosEmPe()
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(40, 10);

            // Act
            var novaCapacidade = capacidade.AtualizarPassageirosEmPe(15);

            // Assert
            novaCapacidade.AssentosDisponiveis.Should().Be(40);
            novaCapacidade.PassageirosEmPe.Should().Be(15);
            novaCapacidade.CapacidadeMaxima.Should().Be(55);
        }

        [Theory]
        [InlineData(40, 0, "40 assentos")]
        [InlineData(40, 10, "40 assentos + 10 em pé = 50 total")]
        public void CapacidadeVeiculo_DeveFormatarDescricaoCorretamente(int assentos, int emPe, string descricaoEsperada)
        {
            // Arrange
            var capacidade = new CapacidadeVeiculo(assentos, emPe);

            // Act
            var descricao = capacidade.DescricaoCapacidade;

            // Assert
            descricao.Should().Be(descricaoEsperada);
        }

        [Fact]
        public void CapacidadeVeiculo_DeveImplementarEqualsEGetHashCode()
        {
            // Arrange
            var capacidade1 = new CapacidadeVeiculo(40, 10);
            var capacidade2 = new CapacidadeVeiculo(40, 10);
            var capacidade3 = new CapacidadeVeiculo(50, 10);

            // Act & Assert
            capacidade1.Should().Be(capacidade2);
            capacidade1.Should().NotBe(capacidade3);
            capacidade1.GetHashCode().Should().Be(capacidade2.GetHashCode());
            capacidade1.GetHashCode().Should().NotBe(capacidade3.GetHashCode());
        }
    }
}
