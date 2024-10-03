using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Data;

namespace Dominio.Entidades.Viagens.Gatilho
{
    public class GatilhoViagem : BaseEntity
    {
        public string Descricao { get; }
        public virtual Veiculo Veiculo { get; }
        public long VeiculoId { get; }
        public virtual Motorista Motorista { get; }
        public long MotoristaId { get; }
        public virtual Localidade Origem { get; }
        public long OrigemId { get; }
        public virtual Localidade Destino { get; }
        public long DestinoId { get; }
        public DateTime HorarioSaida { get; }
        public DateTime HorarioChegada { get; }
        public string DescricaoViagem { get; }
        public decimal Distancia { get; }
        public string PolilinhaRota { get; }
        public bool Ativo { get; }
        public virtual ICollection<Viagem> Viagem { get; }
        public List<DiaSemanaEnum> DiasSemana { get; }

        #region Propriedades Virtuais

        protected override string DescricaoFormatada
        {
            get { return Descricao; }
        }

        #endregion

    }
}
