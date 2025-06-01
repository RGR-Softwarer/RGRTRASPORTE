using System;

namespace Dominio.Dtos.Viagens
{
    public class ViagemCriadaJobData : ViagemJobDataBase
    {
        public new long ViagemId { get; set; }
        public DateTime DataCriacao { get; set; }
        public string UsuarioCriacao { get; set; }
    }
}
