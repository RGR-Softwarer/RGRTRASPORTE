using Dominio.Exceptions;
using Dominio.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Teste.Entidades.Dominio.ValueObjects
{
    public class CPFTests
    {
        [Theory]
        [InlineData("11823444962")]       
        public void CPF_DeveCriarComCpfValido(string numeroCpf)
        {
            // Arrange & Act
            var cpf = new CPF(numeroCpf);

            // Assert
            cpf.Numero.Should().Be(numeroCpf);
            cpf.NumeroFormatado.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData("118.234.449-62")]        
        public void CPF_DeveRemoverFormatacao(string numeroCpfFormatado)
        {
            // Arrange
            var numeroLimpo = numeroCpfFormatado.Replace(".", "").Replace("-", "");

            // Act
            var cpf = new CPF(numeroCpfFormatado);

            // Assert
            cpf.Numero.Should().Be(numeroLimpo);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CPF_DeveLancarExcecao_QuandoCpfVazioOuNulo(string numeroCpf)
        {
            // Arrange & Act & Assert
            Action act = () => new CPF(numeroCpf);
            act.Should().Throw<DomainException>()
                .WithMessage("CPF é obrigatório.");
        }

        [Theory]
        [InlineData("1234567890")]  // 10 dígitos
        [InlineData("123456789012")] // 12 dígitos
        [InlineData("123")]          // 3 dígitos
        public void CPF_DeveLancarExcecao_QuandoTamanhoInvalido(string numeroCpf)
        {
            // Arrange & Act & Assert
            Action act = () => new CPF(numeroCpf);
            act.Should().Throw<DomainException>()
                .WithMessage("CPF deve ter 11 caracteres.");
        }

        [Theory]
        [InlineData("1234567890a")] // Contém letra
        [InlineData("123456789#0")] // Contém símbolo
        [InlineData("12345 67890")] // Contém espaço
        public void CPF_DeveLancarExcecao_QuandoContemCaracteresInvalidos(string numeroCpf)
        {
            // Arrange & Act & Assert
            Action act = () => new CPF(numeroCpf);
            act.Should().Throw<DomainException>()
                .WithMessage("CPF deve conter apenas números.");
        }

        [Theory]
        [InlineData("11111111111")] // Todos iguais
        [InlineData("22222222222")] // Todos iguais
        [InlineData("12345678900")] // CPF inválido
        public void CPF_DeveLancarExcecao_QuandoCpfInvalido(string numeroCpf)
        {
            // Arrange & Act & Assert
            Action act = () => new CPF(numeroCpf);
            act.Should().Throw<DomainException>()
                .WithMessage("CPF inválido.");
        }

        [Theory]
        [InlineData("11144477735", true)]
        [InlineData("12345678909", true)]
        [InlineData("11111111111", false)]
        [InlineData("12345678900", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public void ValidarCpf_DeveRetornarResultadoCorreto(string numeroCpf, bool esperado)
        {
            // Act
            var resultado = CPF.ValidarCpf(numeroCpf);

            // Assert
            resultado.Should().Be(esperado);
        }

        [Fact]
        public void NumeroFormatado_DeveRetornarCpfComMascara()
        {
            // Arrange
            var cpf = new CPF("11144477735");

            // Act
            var formatado = cpf.NumeroFormatado;

            // Assert
            formatado.Should().Be("111.444.777-35");
        }

        [Fact]
        public void CPF_DeveSerIgual_QuandoNumerosIguais()
        {
            // Arrange
            var cpf1 = new CPF("11144477735");
            var cpf2 = new CPF("111.444.777-35");

            // Act & Assert
            cpf1.Should().Be(cpf2);
            (cpf1.Numero == cpf2.Numero).Should().BeTrue();
            (cpf1.Numero != cpf2.Numero).Should().BeFalse();
        }

        [Fact]
        public void CPF_DeveSerDiferente_QuandoNumerosDiferentes()
        {
            // Arrange
            var cpf1 = new CPF("11144477735");
            var cpf2 = new CPF("12345678909");

            // Act & Assert
            cpf1.Should().NotBe(cpf2);
            (cpf1 == cpf2).Should().BeFalse();
            (cpf1 != cpf2).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_DeveRetornarHashIgual_ParaCpfsIguais()
        {
            // Arrange
            var cpf1 = new CPF("11144477735");
            var cpf2 = new CPF("111.444.777-35");

            // Act & Assert
            cpf1.GetHashCode().Should().Be(cpf2.GetHashCode());
        }

        [Fact]
        public void ToString_DeveRetornarNumeroFormatado()
        {
            // Arrange
            var cpf = new CPF("11144477735");

            // Act
            var resultado = cpf.ToString();

            // Assert
            resultado.Should().Be("111.444.777-35");
        }
    }
}