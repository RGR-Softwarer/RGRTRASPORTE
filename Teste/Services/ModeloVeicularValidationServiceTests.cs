using Dominio.Services;
using Dominio.Enums.Veiculo;
using Dominio.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Teste.Services
{
    public class ModeloVeicularValidationServiceTests
    {
        private readonly ModeloVeicularValidationService _service;
        private readonly Mock<INotificationContext> _notificationContextMock;

        public ModeloVeicularValidationServiceTests()
        {
            _service = new ModeloVeicularValidationService();
            _notificationContextMock = new Mock<INotificationContext>();
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarTrue_QuandoDadosValidos()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeTrue();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoDescricaoVazia()
        {
            // Arrange
            var descricao = "";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoQuantidadeAssentoInvalida()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = -1;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoQuantidadeEixoInvalida()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 0;
            var capacidadeMaxima = 50;
            var passageirosEmPe = 10;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoCapacidadeMaximaInvalida()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = -1;
            var passageirosEmPe = 10;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoPassageirosEmPeInvalido()
        {
            // Arrange
            var descricao = "Modelo Teste";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = 40;
            var quantidadeEixo = 2;
            var capacidadeMaxima = 50;
            var passageirosEmPe = -1;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public void ValidarCriacao_DeveRetornarFalse_QuandoMultiplosErros()
        {
            // Arrange
            var descricao = "";
            var tipo = TipoModeloVeiculoEnum.Onibus;
            var quantidadeAssento = -1;
            var quantidadeEixo = 0;
            var capacidadeMaxima = -1;
            var passageirosEmPe = -1;

            // Act
            var resultado = _service.ValidarCriacao(descricao, tipo, quantidadeAssento, quantidadeEixo,
                capacidadeMaxima, passageirosEmPe, true, true, _notificationContextMock.Object);

            // Assert
            resultado.Should().BeFalse();
            _notificationContextMock.Verify(x => x.AddNotification(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}