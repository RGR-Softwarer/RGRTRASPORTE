using Dominio.Exceptions;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio
{
    public class DomainExceptionTests
    {
        [Fact]
        public void DomainException_DeveCriarComMensagem()
        {
            // Arrange
            var mensagem = "Erro de domínio";

            // Act
            var exception = new DomainException(mensagem);

            // Assert
            exception.Message.Should().Be(mensagem);
        }

        [Fact]
        public void DomainException_DeveCriarComMensagemEInnerException()
        {
            // Arrange
            var mensagem = "Erro de domínio";
            var innerException = new Exception("Erro interno");

            // Act
            var exception = new DomainException(mensagem, innerException);

            // Assert
            exception.Message.Should().Be(mensagem);
            exception.InnerException.Should().Be(innerException);
        }

        [Fact]
        public void DomainException_DeveHerdarDeException()
        {
            // Arrange & Act
            var exception = new DomainException("Teste");

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }
    }
}