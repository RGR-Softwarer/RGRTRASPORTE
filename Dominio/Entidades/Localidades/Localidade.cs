namespace Dominio.Entidades.Localidades
{
    public class Localidade : BaseEntity
    {
        public string Nome { get; }
        public string Cep { get; }
        public string Uf { get; }
        public string Cidade { get; }
        public string Bairro { get; }
        public string Logradouro { get; }
        public string Complemento { get; }
        public string Numero { get; }
        public string Latitude { get; }
        public string Longitude { get; }
    }
}
