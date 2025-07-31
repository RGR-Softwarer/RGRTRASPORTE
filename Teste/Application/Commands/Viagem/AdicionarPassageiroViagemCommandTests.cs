using Application.Commands.Viagem;
using Dominio.Enums.Pessoas;
using FluentAssertions;
using Xunit;

namespace Teste.Application.Commands.Viagem
{
    public class AdicionarPassageiroViagemCommandTests
    {
        [Fact]
        public void AdicionarPassageiroViagemCommand_DeveCriarComDadosValidos()
        {
            // Arrange
            var viagemId = 1L;
            var passageiroId = 2L;

            // Act
            var command = new AdicionarPassageiroViagemCommand(viagemId, passageiroId);

            // Assert
            command.Should().NotBeNull();
        }
    }
}