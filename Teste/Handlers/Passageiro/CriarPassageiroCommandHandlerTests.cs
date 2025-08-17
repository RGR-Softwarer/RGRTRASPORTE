using Application.Commands.Passageiro;
using Dominio.Enums.Pessoas;
using Dominio.Interfaces.Infra.Data.Passageiros;
using Dominio.Interfaces;
using FluentAssertions;

using Microsoft.Extensions.Logging;
using Moq;

namespace Teste.Handlers.Passageiro
{
    public class CriarPassageiroCommandHandlerTests
    {
        private readonly Mock<IPassageiroRepository> _passageiroRepositoryMock;
        private readonly Mock<ILogger<CriarPassageiroCommandHandler>> _loggerMock;
        private readonly Mock<INotificationContext> _notificationContextMock;
        private readonly CriarPassageiroCommandHandler _handler;
        public CriarPassageiroCommandHandlerTests()
        {
            _passageiroRepositoryMock = new Mock<IPassageiroRepository>();
            _loggerMock = new Mock<ILogger<CriarPassageiroCommandHandler>>();
            _notificationContextMock = new Mock<INotificationContext>();

            _handler = new CriarPassageiroCommandHandler(
                _passageiroRepositoryMock.Object,
                _loggerMock.Object,
                _notificationContextMock.Object);
        }



        [Fact]
        public async Task Handle_DeveCriarPassageiro_QuandoComandoValido()
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                "11144477735",
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            _passageiroRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Dominio.Entidades.Pessoas.Passageiros.Passageiro>(), It.IsAny<CancellationToken>()))
                    .Callback<Dominio.Entidades.Pessoas.Passageiros.Passageiro, CancellationToken>((p, _) => p.Id = 1)
                    .Returns(Task.CompletedTask);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sucesso.Should().BeTrue();
            result.Dados.Should().BeGreaterThan(0);

            _passageiroRepositoryMock.Verify(
                x => x.AdicionarAsync(It.IsAny<Dominio.Entidades.Pessoas.Passageiros.Passageiro>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalha_QuandoRepositorioFalha()
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                "11144477735",
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            _passageiroRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Dominio.Entidades.Pessoas.Passageiros.Passageiro>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Erro no repositório"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().NotBeEmpty();
            result.Erros.Should().Contain("Erro no repositório");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task Handle_DeveRetornarFalha_QuandoNomeInvalido(string nome)
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                nome,
                "11144477735",
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("cpf-inválido")]
        [InlineData("12345")]
        public async Task Handle_DeveRetornarFalha_QuandoCpfInvalido(string cpf)
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                cpf,
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("email-inválido")]
        [InlineData("@email.com")]
        public async Task Handle_DeveRetornarFalha_QuandoEmailInvalido(string email)
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                "11144477735",
                "(11) 99999-9999",
                email,
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Sucesso.Should().BeFalse();
            result.Erros.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_DeveLogarInformacao_QuandoPassageiroForCriado()
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                "11144477735",
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            _passageiroRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Dominio.Entidades.Pessoas.Passageiros.Passageiro>(), It.IsAny<CancellationToken>()))
                .Callback<Dominio.Entidades.Pessoas.Passageiros.Passageiro, CancellationToken>((p, _) => p.Id = 1)
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _loggerMock.Verify(    
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("João Silva criado com sucesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);

        }

        [Fact]
        public async Task Handle_DeveLogarErro_QuandoOcorrerExcecao()
        {
            // Arrange
            var command = new CriarPassageiroCommand(
                "João Silva",
                "11144477735",
                "(11) 99999-9999",
                "joao@email.com",
                SexoEnum.Masculino,
                1, 2, 3,
                "Observação teste",
                true);

            var exception = new Exception("Erro no repositório");
            _passageiroRepositoryMock
                .Setup(x => x.AdicionarAsync(It.IsAny<Dominio.Entidades.Pessoas.Passageiros.Passageiro>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao criar passageiro")),
                    It.Is<Exception>(e => e == exception),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}