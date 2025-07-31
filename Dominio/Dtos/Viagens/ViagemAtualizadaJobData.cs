using System;

namespace Dominio.Dtos.Viagens
{
    public class ViagemAtualizadaJobData : ViagemJobDataBase
    {
        public new long ViagemId { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
    }
} 
