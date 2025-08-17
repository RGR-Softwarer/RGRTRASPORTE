using Dominio.Entidades.Veiculos;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Teste.Entidades.Dominio
{
    public class ModeloVeicularFactoryTests
    {
        [Fact]
        public void CriarModeloVeicular_DeveCriarComDadosValidos()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;
            var possuiBanheiro = true;
            var possuiClimatizador = true;

            // Act
            var modelo = ModeloVeicular.CriarModeloVeicular(
                descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, possuiBanheiro, possuiClimatizador);

            // Assert
            modelo.Should().NotBeNull();
            modelo.Descricao.Should().Be(descricao);
            modelo.Tipo.Should().Be(tipo);
            modelo.QuantidadeAssento.Should().Be(quantidadeAssento);
            modelo.QuantidadeEixo.Should().Be(quantidadeEixo);
            modelo.CapacidadeMaxima.Should().Be(capacidadeMaxima);
            modelo.PassageirosEmPe.Should().Be(passageirosEmPe);
            modelo.PossuiBanheiro.Should().Be(possuiBanheiro);
            modelo.PossuiClimatizador.Should().Be(possuiClimatizador);
            modelo.Situacao.Should().BeTrue();
        }

        [Fact]
        public void CriarModeloVeicularComValidacao_DeveRetornarSucesso_QuandoDadosValidos()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;
            var possuiBanheiro = true;
            var possuiClimatizador = true;
            var notificationContextMock = new Mock<IDomainNotificationContext>();

            // Act
            var (modelo, sucesso) = ModeloVeicular.CriarModeloVeicularComValidacao(
                descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, possuiBanheiro, possuiClimatizador, 
                notificationContextMock.Object);

            // Assert
            sucesso.Should().BeTrue();
            modelo.Should().NotBeNull();
            modelo!.Descricao.Should().Be(descricao);
            notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CriarModeloVeicularComValidacao_DeveRetornarFalha_QuandoDadosInvalidos()
        {
            // Arrange
            var descricao = ""; // Descrição inválida
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = -1; // Quantidade inválida
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;
            var possuiBanheiro = true;
            var possuiClimatizador = true;
            var notificationContextMock = new Mock<IDomainNotificationContext>();

            // Act
            var (modelo, sucesso) = ModeloVeicular.CriarModeloVeicularComValidacao(
                descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, possuiBanheiro, possuiClimatizador, 
                notificationContextMock.Object);

            // Assert
            sucesso.Should().BeFalse();
            modelo.Should().BeNull();
            notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}