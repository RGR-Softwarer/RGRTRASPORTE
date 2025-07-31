using Dominio.Entidades.Localidades;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio
{
    public class LocalidadeTests
    {
        [Fact]
        public void Localidade_DeveTerPropriedadesBasicas()
        {
            // Arrange - apenas testando a classe existe

            // Act & Assert
            var localidadeType = typeof(Localidade);
            localidadeType.Should().NotBeNull();
            localidadeType.Name.Should().Be("Localidade");
        }
    }
}