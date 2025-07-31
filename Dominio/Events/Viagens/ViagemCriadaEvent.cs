using Dominio.Events.Base;
using MediatR;

namespace Dominio.Events.Viagens
{
    public class ViagemCriadaEvent : DomainEvent, INotification
    {
        public long ViagemId { get; private set; }
        public long VeiculoId { get; private set; }
        public long MotoristaId { get; private set; }
        public long LocalidadeOrigemId { get; private set; }
        public long LocalidadeDestinoId { get; private set; }
        public DateTime DataCriacao { get; private set; }

        public ViagemCriadaEvent(
            long viagemId,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId)
        {
            ViagemId = viagemId;
            VeiculoId = veiculoId;
            MotoristaId = motoristaId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            DataCriacao = DateTime.UtcNow;
        }
    }
} 
