using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Viagens;

namespace Dominio.Entidades.Viagens
{
    public class Viagem : BaseEntity
    {
        public string CodigoViagem { get; }
        //public DateTime DataCriacao { get; set; }
        public DateTime DataViagem { get; }
        //public DateTime? DataAtualizacao { get; set; }
        public virtual Veiculo Veiculo { get; }
        public long VeiculoId { get; }
        public virtual Motorista Motorista { get; }
        public long MotoristaId { get; }
        public virtual Localidade Origem { get; }
        public long OrigemId { get; }
        public virtual Localidade Destino { get; }
        public long DestinoId { get; }
        public virtual GatilhoViagem GatinhoViagem { get; }
        public long? GatinhoViagemId { get; }
        public DateTime HorarioSaida { get; }
        public DateTime HorarioChegada { get; }
        public SituacaoViagemEnum Situacao { get; }
        public string MotivoProblema { get; }
        public string DescricaoViagem { get; }
        public DateTime? DataInicioViagem { get; }
        public decimal? LatitudeInicioViagem { get; }
        public decimal? LongitudeInicioViagem { get; }
        public DateTime? DataFimViagem { get; }
        public decimal? LatitudeFimViagem { get; }
        public decimal? LongitudeFimViagem { get; }
        public decimal Distancia { get; }
        public decimal? DistanciaRealizada { get; }
        public string PolilinhaRota { get; }
        public string? PolilinhaRotaRealizada { get; }
        //public Dominio.Entidades.Programacao.ProgramacaoViagemPosicao UltimaPosicao { get; set; }

        public int NumeroPassageiros { get; }
        public bool Lotado { get; }
        public bool Excesso { get; }

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
