using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Viagens;

namespace Dominio.Entidades.Viagens
{
    public class Viagem : BaseEntity
    {
        public string CodigoViagem { get; private set; }
        //public DateTime DataCriacao { get; set; }
        public DateTime DataViagem { get; private set; }
        //public DateTime? DataAtualizacao { get; set; }
        public virtual Veiculo Veiculo { get; private set; }
        public long VeiculoId { get; private set; }
        public virtual Motorista Motorista { get; private set; }
        public long MotoristaId { get; private set; }
        public virtual Localidade Origem { get; private set; }
        public long OrigemId { get; private set; }
        public virtual Localidade Destino { get; private set; }
        public long DestinoId { get; private set; }
        public virtual GatilhoViagem GatilhoViagem { get; private set; }
        public long? GatilhoViagemId { get; private set; }
        public DateTime HorarioSaida { get; private set; }
        public DateTime HorarioChegada { get; private set; }
        public SituacaoViagemEnum Situacao { get; private set; }
        public string MotivoProblema { get; private set; }
        public string DescricaoViagem { get; private set; }
        public DateTime? DataInicioViagem { get; private set; }
        public decimal? LatitudeInicioViagem { get; private set; }
        public decimal? LongitudeInicioViagem { get; private set; }
        public DateTime? DataFimViagem { get; private set; }
        public decimal? LatitudeFimViagem { get; private set; }
        public decimal? LongitudeFimViagem { get; private set; }
        public decimal Distancia { get; private set; }
        public decimal? DistanciaRealizada { get; private set; }
        public string PolilinhaRota { get; private set; }
        public string? PolilinhaRotaRealizada { get; private set; }
        //public Dominio.Entidades.Programacao.ProgramacaoViagemPosicao UltimaPosicao { get; set; }

        public int NumeroPassageiros { get; private set; }
        public bool Lotado { get; private set; }
        public bool Excesso { get; private set; }

        #region Propriedades Virtuais

        protected override string DescricaoFormatada
        {
            get { return CodigoViagem.ToString(); }
        }

        public virtual string ExcessoFormatado
        {
            get { return Excesso ? "Sim" : "Não"; }
        }

        #endregion

    }
}
