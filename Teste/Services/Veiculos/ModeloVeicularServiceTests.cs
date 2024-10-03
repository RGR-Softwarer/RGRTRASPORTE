using Moq;
using Xunit;
using Service;
using Dominio.Entidades.Veiculos;
using Dominio.Dtos.Veiculo;
using AutoMapper;
using Infra.CrossCutting.Handlers.Notifications;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dominio.Dtos.Auditoria;
using Dominio.Enums.Veiculo;
using Service.Services;
using Dominio.Interfaces.Infra.Data.Veiculo;

namespace Teste.Services.Veiculos
{
    public class ModeloVeicularServiceTests
    {
        private readonly Mock<IModeloVeicularRepository> _modeloVeicularRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly ModeloVeicularService _modeloVeicularService;

        public ModeloVeicularServiceTests()
        {
            _modeloVeicularRepositoryMock = new Mock<IModeloVeicularRepository>();
            _mapperMock = new Mock<IMapper>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _modeloVeicularService = new ModeloVeicularService(_notificationHandlerMock.Object, _modeloVeicularRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarListaDeModelosVeicularesDto_QuandoExistiremModelos()
        {
            // Arrange
            var modelos = new List<ModeloVeicular>
            {
                new ModeloVeicular(),
                new ModeloVeicular()
            };

            var modelosDto = new List<ModeloVeicularDto>
            {
                new ModeloVeicularDto
                {
                    Id = 1,
                    Situacao = true,
                    DescricaoModelo = true,
                    Tipo = TipoModeloVeiculoEnum.Van,
                    QuantidadeAssento = 4,
                    QuantidadeEixo = 2,
                    CapacidadeMaxima = 1000,
                    PassageirosEmPe = 0,
                    PossuiBanheiro = false,
                    PossuiClimatizador = true,
                },
                new ModeloVeicularDto
                {
                    Id = 2,
                    Situacao = false,
                    DescricaoModelo = false,
                    Tipo = TipoModeloVeiculoEnum.Van,
                    QuantidadeAssento = 2,
                    QuantidadeEixo = 4,
                    CapacidadeMaxima = 5000,
                    PassageirosEmPe = 0,
                    PossuiBanheiro = false,
                    PossuiClimatizador = false,
                }
            };

            _modeloVeicularRepositoryMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(modelos);
            _mapperMock.Setup(m => m.Map<List<ModeloVeicularDto>>(modelos)).Returns(modelosDto);

            // Act
            var resultado = await _modeloVeicularService.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarModeloVeicularDto_QuandoModeloExistir()
        {
            // Arrange
            var modelo = new ModeloVeicular();
            var modeloDto = new ModeloVeicularDto
            {
                Id = 1,
                Situacao = true,
                DescricaoModelo = true,
                Tipo = TipoModeloVeiculoEnum.Van,
                QuantidadeAssento = 4,
                QuantidadeEixo = 2,
                CapacidadeMaxima = 1000,
                PassageirosEmPe = 0,
                PossuiBanheiro = false,
                PossuiClimatizador = true,
            };

            _modeloVeicularRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(modelo);
            _mapperMock.Setup(m => m.Map<ModeloVeicularDto>(modelo)).Returns(modeloDto);

            // Act
            var resultado = await _modeloVeicularService.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarModeloVeicular_QuandoDtoValido()
        {
            // Arrange
            var modeloDto = new ModeloVeicularDto
            {
                Situacao = true,
                DescricaoModelo = true,
                Tipo = TipoModeloVeiculoEnum.Van,
                QuantidadeAssento = 4,
                QuantidadeEixo = 2,
                CapacidadeMaxima = 1000,
                PassageirosEmPe = 0,
                PossuiBanheiro = false,
                PossuiClimatizador = true
            };
            var modelo = new ModeloVeicular();

            _mapperMock.Setup(m => m.Map<ModeloVeicular>(modeloDto)).Returns(modelo);

            // Act
            await _modeloVeicularService.AdicionarAsync(modeloDto);

            // Assert
            _modeloVeicularRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<ModeloVeicular>(), It.IsAny<AuditadoDto>()), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverModeloVeicular_QuandoModeloExistir()
        {
            // Arrange
            var modelo = new ModeloVeicular();

            _modeloVeicularRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(modelo);
            _modeloVeicularRepositoryMock.Setup(r => r.RemoverAsync(modelo));

            // Act
            await _modeloVeicularService.RemoverAsync(1);

            // Assert
            _modeloVeicularRepositoryMock.Verify(r => r.RemoverAsync(It.IsAny<ModeloVeicular>()), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveNotificar_QuandoModeloNaoExistir()
        {
            // Arrange
            _modeloVeicularRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(null as ModeloVeicular);

            // Act
            await _modeloVeicularService.RemoverAsync(1);

            // Assert
            _notificationHandlerMock.Verify(n => n.AddNotification(It.IsAny<string>(), It.IsAny<System.Net.HttpStatusCode>()), Times.Once);
            _modeloVeicularRepositoryMock.Verify(r => r.RemoverAsync(It.IsAny<ModeloVeicular>()), Times.Never);
        }
    }
}
