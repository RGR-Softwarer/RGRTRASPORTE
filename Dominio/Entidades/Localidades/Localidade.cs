using Dominio.Exceptions;

namespace Dominio.Entidades.Localidades
{
    public class Localidade : BaseEntity
    {
        protected Localidade() { } // Construtor protegido para EF Core

        public Localidade(
            string nome,
            string estado,
            string cidade,
            string cep,
            string bairro,
            string logradouro,
            string numero,
            string complemento,
            decimal latitude,
            decimal longitude)
        {
            ValidarNome(nome);
            ValidarEstado(estado);
            ValidarCidade(cidade);
            ValidarCEP(cep);
            ValidarBairro(bairro);
            ValidarLogradouro(logradouro);
            ValidarNumero(numero);
            ValidarCoordenadas(latitude, longitude);

            Nome = nome;
            Estado = estado;
            Cidade = cidade;
            Cep = cep;
            Bairro = bairro;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Latitude = latitude;
            Longitude = longitude;
            Ativo = true;
        }

        public string Nome { get; private set; }
        public string Estado { get; private set; }
        public string Cidade { get; private set; }
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public bool Ativo { get; private set; }

        public string Uf => Estado?.ToUpperInvariant();

        public string EnderecoCompleto => $"{Logradouro}, {Numero}{(string.IsNullOrWhiteSpace(Complemento) ? "" : $" - {Complemento}")}, {Bairro}, {Cidade}/{Estado}, CEP: {Cep}";

        public void Atualizar(
            string nome,
            string estado,
            string cidade,
            string cep,
            string bairro,
            string logradouro,
            string numero,
            string complemento,
            decimal latitude,
            decimal longitude,
            bool ativo)
        {
            ValidarNome(nome);
            ValidarEstado(estado);
            ValidarCidade(cidade);
            ValidarCEP(cep);
            ValidarBairro(bairro);
            ValidarLogradouro(logradouro);
            ValidarNumero(numero);
            ValidarCoordenadas(latitude, longitude);

            Nome = nome;
            Estado = estado;
            Cidade = cidade;
            Cep = cep;
            Bairro = bairro;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void Ativar()
        {
            if (Ativo)
                throw new DomainException("Localidade já está ativa.");

            Ativo = true;
        }

        public void Inativar()
        {
            if (!Ativo)
                throw new DomainException("Localidade já está inativa.");

            Ativo = false;
        }

        protected override string DescricaoFormatada => $"{Nome} - {Cidade}/{Estado}";

        private void ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório.");

            if (nome.Length > 100)
                throw new DomainException("Nome deve ter no máximo 100 caracteres.");
        }

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

        private void ValidarCoordenadas(decimal latitude, decimal longitude)
        {
            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude deve estar entre -90 e 90.");

            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude deve estar entre -180 e 180.");
        }
    }
}
