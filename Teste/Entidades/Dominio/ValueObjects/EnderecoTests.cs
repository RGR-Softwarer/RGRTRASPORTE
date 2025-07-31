using Dominio.Exceptions;
using Dominio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class EnderecoTests
    {
        [Fact]
        public void Endereco_DeveCriarComDadosValidos()
        {
            // Arrange
            var estado = "SP";
            var cidade = "São Paulo";
            var cep = "01234567";
            var bairro = "Centro";
            var logradouro = "Rua das Flores";
            var numero = "123";
            var complemento = "Apto 45";

            // Act
            var endereco = new Endereco(estado, cidade, cep, bairro, logradouro, numero, complemento);

            // Assert
            endereco.Estado.Should().Be(estado);
            endereco.Cidade.Should().Be(cidade);
            endereco.Cep.Should().Be(cep);
            endereco.Bairro.Should().Be(bairro);
            endereco.Logradouro.Should().Be(logradouro);
            endereco.Numero.Should().Be(numero);
            endereco.Complemento.Should().Be(complemento);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Endereco_DeveLancarExcecao_QuandoLogradouroInvalido(string logradouro)
        {
            // Act & Assert
            Action act = () => new Endereco("SP", "São Paulo", "01234567", "Centro", logradouro, "123");
            act.Should().Throw<DomainException>()
                .WithMessage("Logradouro é obrigatório.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Endereco_DeveLancarExcecao_QuandoNumeroInvalido(string numero)
        {
            // Act & Assert
            Action act = () => new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", numero);
            act.Should().Throw<DomainException>()
                .WithMessage("Número é obrigatório.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Endereco_DeveLancarExcecao_QuandoBairroInvalido(string bairro)
        {
            // Act & Assert
            Action act = () => new Endereco("SP", "São Paulo", "01234567", bairro, "Rua das Flores", "123");
            act.Should().Throw<DomainException>()
                .WithMessage("Bairro é obrigatório.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Endereco_DeveLancarExcecao_QuandoCidadeInvalida(string cidade)
        {
            // Act & Assert
            Action act = () => new Endereco("SP", cidade, "01234567", "Centro", "Rua das Flores", "123");
            act.Should().Throw<DomainException>()
                .WithMessage("Cidade é obrigatória.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Endereco_DeveLancarExcecao_QuandoEstadoInvalido(string estado)
        {
            // Act & Assert
            Action act = () => new Endereco(estado, "São Paulo", "01234567", "Centro", "Rua das Flores", "123");
            act.Should().Throw<DomainException>()
                .WithMessage("Estado é obrigatório.");
        }

        [Theory]
        [InlineData("1234567")] // 7 dígitos
        [InlineData("123456789")] // 9 dígitos
        [InlineData("1234567a")] // Com letra
        public void Endereco_DeveLancarExcecao_QuandoCepInvalido(string cep)
        {
            // Act & Assert
            Action act = () => new Endereco("SP", "São Paulo", cep, "Centro", "Rua das Flores", "123");
            act.Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("01234567")]
        [InlineData("12345678")]
        public void Endereco_DeveCriarComCepValido(string cep)
        {
            // Act
            var endereco = new Endereco("SP", "São Paulo", cep, "Centro", "Rua das Flores", "123");

            // Assert
            endereco.Cep.Should().Be(cep);
        }

        [Fact]
        public void EnderecoCompleto_DeveRetornarEnderecoFormatado()
        {
            // Arrange
            var endereco = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123", "Apto 45");

            // Act
            var enderecoCompleto = endereco.EnderecoCompleto;

            // Assert
            enderecoCompleto.Should().Be("Rua das Flores, 123 - Apto 45, Centro, São Paulo/SP, CEP: 01234567");
        }

        [Fact]
        public void EnderecoCompleto_DeveRetornarEnderecoFormatado_SemComplemento()
        {
            // Arrange
            var endereco = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");

            // Act
            var enderecoCompleto = endereco.EnderecoCompleto;

            // Assert
            enderecoCompleto.Should().Be("Rua das Flores, 123, Centro, São Paulo/SP, CEP: 01234567");
        }

        [Fact]
        public void Cep_DeveRetornarCepSemMascara()
        {
            // Arrange
            var endereco = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");

            // Act
            var cep = endereco.Cep;

            // Assert
            cep.Should().Be("01234567");
        }

        [Fact]
        public void Endereco_DeveSerIgual_QuandoTodosCamposIguais()
        {
            // Arrange
            var endereco1 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123", "Apto 45");
            var endereco2 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123", "Apto 45");

            // Act & Assert
            endereco1.Should().Be(endereco2);
            (endereco1.EnderecoCompleto == endereco2.EnderecoCompleto).Should().BeTrue();
            (endereco1.EnderecoCompleto != endereco2.EnderecoCompleto).Should().BeFalse();
        }

        [Fact]
        public void Endereco_DeveSerDiferente_QuandoCamposDiferentes()
        {
            // Arrange
            var endereco1 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");
            var endereco2 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Rosas", "456");

            // Act & Assert
            endereco1.Should().NotBe(endereco2);
            (endereco1 == endereco2).Should().BeFalse();
            (endereco1 != endereco2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_DeveRetornarHashIgual_ParaEnderecosIguais()
        {
            // Arrange
            var endereco1 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");
            var endereco2 = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");

            // Act & Assert
            endereco1.GetHashCode().Should().Be(endereco2.GetHashCode());
        }

        [Fact]
        public void ToString_DeveRetornarEnderecoCompleto()
        {
            // Arrange
            var endereco = new Endereco("SP", "São Paulo", "01234567", "Centro", "Rua das Flores", "123");

            // Act
            var resultado = endereco.ToString();

            // Assert
            resultado.Should().Be(endereco.EnderecoCompleto);
        }
    }
}