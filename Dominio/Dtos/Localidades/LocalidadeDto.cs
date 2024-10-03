﻿namespace Dominio.Dtos.Localidades
{
    public class LocalidadeDto
    {
        public long Id { get; set; }
        public string Nome { get; init; }
        public string Cep { get; init; }
        public string Uf { get; init; }
        public string Cidade { get; init; }
        public string Bairro { get; init; }
        public string Logradouro { get; init; }
        public string Complemento { get; init; }
        public string Numero { get; init; }
        public string Latitude { get; init; }
        public string Longitude { get; init; }
    }
}