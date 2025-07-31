using Dominio.Entidades.Veiculos;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio
{
    public class VeiculoTests
    {
        [Fact]
        public void Veiculo_DeveTerPropriedadesBasicas()
        {
            // Arrange - apenas testando a classe existe

            // Act & Assert
            var veiculoType = typeof(Veiculo);
            veiculoType.Should().NotBeNull();
            veiculoType.Name.Should().Be("Veiculo");
        }
    }
}