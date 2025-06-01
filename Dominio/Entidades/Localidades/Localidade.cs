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
            Ativo = ativo;
        }

        protected override string DescricaoFormatada => $"{Nome} - {Cidade}/{Estado}";
    }
}
