using AutoMapper;
using Dominio.Dtos.Auditoria;
using Dominio.Dtos.Veiculo;
using Dominio.Entidades.Veiculo;
using Dominio.Interfaces.Infra.Data;
using Infra.CrossCutting.Handlers.Notifications;
using Moq;
using Service;
using System.Net;

namespace Teste.Services.Veiculos
{
    public class VeiculoServiceTests
    {
        private readonly Mock<IVeiculoRepository> _veiculoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<INotificationHandler> _notificationHandlerMock;
        private readonly VeiculoService _veiculoService;

        public VeiculoServiceTests()
        {
            _veiculoRepositoryMock = new Mock<IVeiculoRepository>();
            _mapperMock = new Mock<IMapper>();
            _notificationHandlerMock = new Mock<INotificationHandler>();
            _veiculoService = new VeiculoService(_notificationHandlerMock.Object, _veiculoRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarListaDeVeiculosDto_QuandoExistiremVeiculos()
        {
            // Arrange
            var veiculos = new List<Veiculo>
            {
                new Veiculo(), // Supondo que você vai usar AutoMapper para preencher os dados
                new Veiculo()
            };

            var veiculosDto = new List<VeiculoDto>
            {
                new VeiculoDto { Id = 1, Modelo = "Modelo1", Marca = "Marca1", Placa = "ABC-1234" },
                new VeiculoDto { Id = 2, Modelo = "Modelo2", Marca = "Marca2", Placa = "DEF-5678" }
            };

            _veiculoRepositoryMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(veiculos);
            _mapperMock.Setup(m => m.Map<List<VeiculoDto>>(veiculos)).Returns(veiculosDto);

            // Act
            var resultado = await _veiculoService.ObterTodosAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Modelo1", resultado[0].Modelo);
            Assert.Equal("Modelo2", resultado[1].Modelo);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarVeiculoDto_QuandoVeiculoExistir()
        {
            // Arrange
            var veiculo = new Veiculo(); // Usando AutoMapper para preencher os dados
            var veiculoDto = new VeiculoDto { Id = 1, Modelo = "Modelo1", Marca = "Marca1", Placa = "ABC-1234" };

            _veiculoRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(veiculo);
            _mapperMock.Setup(m => m.Map<VeiculoDto>(veiculo)).Returns(veiculoDto);

            // Act
            var resultado = await _veiculoService.ObterPorIdAsync(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("Modelo1", resultado.Modelo);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarVeiculo_QuandoDtoValido()
        {
            // Arrange
            var veiculoDto = new VeiculoDto { Modelo = "Modelo1", Marca = "Marca1", Placa = "ABC-1234" };
            var veiculo = new Veiculo(); // Criar uma instância válida de Veiculo

            // Configura o mock para mapear de VeiculoDto para Veiculo
            _mapperMock.Setup(m => m.Map<Veiculo>(veiculoDto)).Returns(veiculo);

            // Act
            await _veiculoService.AdicionarAsync(veiculoDto);

            // Assert
            _veiculoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Veiculo>(), It.IsAny<AuditadoDto>()), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveRemoverVeiculo_QuandoVeiculoExistir()
        {
            // Arrange
            var veiculo = new Veiculo(); // Usando AutoMapper para preencher os dados

            _veiculoRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(veiculo);
            _veiculoRepositoryMock.Setup(r => r.Remover(veiculo));

            // Act
            await _veiculoService.RemoverAsync(1);

            // Assert
            _veiculoRepositoryMock.Verify(r => r.Remover(It.IsAny<Veiculo>()), Times.Once);
        }

        [Fact]
        public async Task RemoverAsync_DeveNotificar_QuandoVeiculoNaoExistir()
        {
            // Arrange
            _veiculoRepositoryMock.Setup(r => r.ObterPorIdAsync(1, false)).ReturnsAsync(null as Veiculo);

            // Act
            await _veiculoService.RemoverAsync(1);

            // Assert
            _notificationHandlerMock.Verify(n => n.AddNotification(It.IsAny<string>(), It.IsAny<HttpStatusCode>()), Times.Once);
            _veiculoRepositoryMock.Verify(r => r.Remover(It.IsAny<Veiculo>()), Times.Never);
        }
    }
}
