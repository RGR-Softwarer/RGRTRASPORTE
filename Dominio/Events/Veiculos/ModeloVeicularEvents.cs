using Dominio.Events.Base;
using Dominio.Enums.Veiculo;

namespace Dominio.Events.Veiculos
{
    public class ModeloVeicularCriadoEvent : DomainEvent
    {
        public ModeloVeicularCriadoEvent(long modeloVeicularId, string descricao, TipoModeloVeiculoEnum tipo)
        {
            ModeloVeicularId = modeloVeicularId;
            Descricao = descricao;
            Tipo = tipo;
        }

        public long ModeloVeicularId { get; }
        public string Descricao { get; }
        public TipoModeloVeiculoEnum Tipo { get; }
    }

    public class ModeloVeicularAtualizadoEvent : DomainEvent
    {
        public ModeloVeicularAtualizadoEvent(long modeloVeicularId, string descricao)
        {
            ModeloVeicularId = modeloVeicularId;
            Descricao = descricao;
        }

        public long ModeloVeicularId { get; }
        public string Descricao { get; }
    }

    public class ModeloVeicularAtivadoEvent : DomainEvent
    {
        public ModeloVeicularAtivadoEvent(long modeloVeicularId, string descricao)
        {
            ModeloVeicularId = modeloVeicularId;
            Descricao = descricao;
        }

        public long ModeloVeicularId { get; }
        public string Descricao { get; }
    }

    public class ModeloVeicularInativadoEvent : DomainEvent
    {
        public ModeloVeicularInativadoEvent(long modeloVeicularId, string descricao)
        {
            ModeloVeicularId = modeloVeicularId;
            Descricao = descricao;
        }

        public long ModeloVeicularId { get; }
        public string Descricao { get; }
    }
} 
