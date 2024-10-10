using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Data;

namespace Dominio.Entidades.Viagens.Gatilho
{
    public class GatilhoViagem : BaseEntity
    {
        public string Descricao { get; private set; }
        public virtual Veiculo Veiculo { get; private set; }
        public long VeiculoId { get; private set; }
        public virtual Motorista Motorista { get; private set; }
        public long MotoristaId { get; private set; }
        public virtual Localidade Origem { get; private set; }
        public long OrigemId { get; private set; }
        public virtual Localidade Destino { get; private set; }
        public long DestinoId { get; private set; }
        public DateTime HorarioSaida { get; private set; }
        public DateTime HorarioChegada { get; private set; }
        public string DescricaoViagem { get; private set; }
        public decimal Distancia { get; private set; }
        public string PolilinhaRota { get; private set; }
        public bool Ativo { get; private set; }
        public virtual ICollection<Viagem> Viagem { get; private set; }
        public List<DiaSemanaEnum> DiasSemana { get; private set; }

        #region Propriedades Virtuais

        protected override string DescricaoFormatada
        {
            get { return Descricao; }
        }

        #endregion

    }
}
