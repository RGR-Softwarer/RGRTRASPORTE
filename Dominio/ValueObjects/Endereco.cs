using Dominio.Exceptions;

namespace Dominio.ValueObjects
{
    public class Endereco
    {
        public string Estado { get; private set; }
        public string Cidade { get; private set; }
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string? Complemento { get; private set; }

        public Endereco(
            string estado,
            string cidade,
            string cep,
            string bairro,
            string logradouro,
            string numero,
            string? complemento = null)
        {
            ValidarEstado(estado);
            ValidarCidade(cidade);
            ValidarCEP(cep);
            ValidarBairro(bairro);
            ValidarLogradouro(logradouro);
            ValidarNumero(numero);

            Estado = estado;
            Cidade = cidade;
            Cep = cep;
            Bairro = bairro;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
        }

        public string Uf => Estado.ToUpperInvariant();

        public string EnderecoCompleto => 
            $"{Logradouro}, {Numero}{(string.IsNullOrWhiteSpace(Complemento) ? "" : $" - {Complemento}")}, {Bairro}, {Cidade}/{Estado}, CEP: {Cep}";

        private void ValidarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado))
                throw new DomainException("Estado é obrigatório.");

            if (estado.Length != 2)
                throw new DomainException("Estado deve ter 2 caracteres.");

            if (!estado.All(char.IsLetter))
                throw new DomainException("Estado deve conter apenas letras.");
        }

        private void ValidarCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                throw new DomainException("Cidade é obrigatória.");

            if (cidade.Length > 100)
                throw new DomainException("Cidade deve ter no máximo 100 caracteres.");
        }

        private void ValidarCEP(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                throw new DomainException("CEP é obrigatório.");

            if (cep.Length != 8)
                throw new DomainException("CEP deve ter 8 caracteres.");

            if (!cep.All(char.IsDigit))
                throw new DomainException("CEP deve conter apenas números.");
        }

        private void ValidarBairro(string bairro)
        {
            if (string.IsNullOrWhiteSpace(bairro))
                throw new DomainException("Bairro é obrigatório.");

            if (bairro.Length > 100)
                throw new DomainException("Bairro deve ter no máximo 100 caracteres.");
        }

        private void ValidarLogradouro(string logradouro)
        {
            if (string.IsNullOrWhiteSpace(logradouro))
                throw new DomainException("Logradouro é obrigatório.");

            if (logradouro.Length > 200)
                throw new DomainException("Logradouro deve ter no máximo 200 caracteres.");
        }

        private void ValidarNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("Número é obrigatório.");

            if (numero.Length > 10)
                throw new DomainException("Número deve ter no máximo 10 caracteres.");
        }

        public override bool Equals(object? obj)
        {
            return obj is Endereco endereco &&
                   Estado == endereco.Estado &&
                   Cidade == endereco.Cidade &&
                   Cep == endereco.Cep &&
                   Bairro == endereco.Bairro &&
                   Logradouro == endereco.Logradouro &&
                   Numero == endereco.Numero &&
                   Complemento == endereco.Complemento;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Estado, Cidade, Cep, Bairro, Logradouro, Numero, Complemento);
        }

        public override string ToString()
        {
            return EnderecoCompleto;
        }
    }
} 