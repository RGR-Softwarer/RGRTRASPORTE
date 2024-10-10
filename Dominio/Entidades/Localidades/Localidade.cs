namespace Dominio.Entidades.Localidades
{
    public class Localidade : BaseEntity
    {
        public string Nome { get; private set ;}
        public string Cep { get; private set;}
        public string Uf { get; private set; }
        public string Cidade { get; private set; }
        public string Bairro { get; private set; }
        public string Logradouro { get; private set; }
        public string Complemento { get; private set; }
        public string Numero { get; private set; }
        public string Latitude { get; private set; }
        public string Longitude { get; private set; }
    }
}
