using Dominio.Enums.Veiculo;
using Dominio.Enums.Pessoas;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio
{
    public class EnumsTests
    {
        [Theory]
        [InlineData(TipoCombustivelEnum.Gasolina)]
        [InlineData(TipoCombustivelEnum.Diesel)]
        [InlineData(TipoCombustivelEnum.Alcool)]
        [InlineData(TipoCombustivelEnum.Eletrico)]
        [InlineData(TipoCombustivelEnum.Hibrido)]
        public void TipoCombustivelEnum_DeveTerValoresDefinidos(TipoCombustivelEnum tipo)
        {
            // Act & Assert
            Enum.IsDefined(typeof(TipoCombustivelEnum), tipo).Should().BeTrue();
        }

        [Theory]
        [InlineData(SexoEnum.Masculino)]
        [InlineData(SexoEnum.Feminino)]
        public void SexoEnum_DeveTerValoresDefinidos(SexoEnum sexo)
        {
            // Act & Assert
            Enum.IsDefined(typeof(SexoEnum), sexo).Should().BeTrue();
        }

        [Theory]
        [InlineData(TipoModeloVeiculoEnum.Onibus)]
        [InlineData(TipoModeloVeiculoEnum.MicroOnibus)]
        [InlineData(TipoModeloVeiculoEnum.Van)]
        [InlineData(TipoModeloVeiculoEnum.Carro)]
        public void TipoModeloVeiculoEnum_DeveTerValoresDefinidos(TipoModeloVeiculoEnum tipo)
        {
            // Act & Assert
            Enum.IsDefined(typeof(TipoModeloVeiculoEnum), tipo).Should().BeTrue();
        }

        [Theory]
        [InlineData(StatusVeiculoEnum.Disponivel)]
        [InlineData(StatusVeiculoEnum.Alugado)]
        [InlineData(StatusVeiculoEnum.EmManutencao)]
        [InlineData(StatusVeiculoEnum.Inativo)]
        public void StatusVeiculoEnum_DeveTerValoresDefinidos(StatusVeiculoEnum status)
        {
            // Act & Assert
            Enum.IsDefined(typeof(StatusVeiculoEnum), status).Should().BeTrue();
        }
    }
}