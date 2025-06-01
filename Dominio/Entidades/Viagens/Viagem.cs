#nullable enable

using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Enums.Viagens;
using Dominio.Exceptions;

namespace Dominio.Entidades.Viagens
{
    public class Viagem : BaseEntity
    {
        protected Viagem() 
        { 
            // Inicializando campos obrigatórios para o EF Core
            CodigoViagem = string.Empty;
            MotivoProblema = string.Empty;
            DescricaoViagem = string.Empty;
            PolilinhaRota = string.Empty;
            PolilinhaRotaRealizada = string.Empty;
            MotivoCancelamento = string.Empty;
        }

        public Viagem(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            decimal valorPassagem,
            int quantidadeVagas,
            decimal distancia,
            string descricaoViagem,
            string polilinhaRota,
            bool ativo)
        {
            CodigoViagem = GerarCodigoViagem();
            DataViagem = dataViagem;
            HorarioSaida = horarioSaida;
            HorarioChegada = horarioChegada;
            VeiculoId = veiculoId;
            MotoristaId = motoristaId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            ValorPassagem = valorPassagem;
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas;
            Distancia = distancia;
            DescricaoViagem = descricaoViagem;
            PolilinhaRota = polilinhaRota;
            NumeroPassageiros = 0;
            Lotado = false;
            Excesso = false;
            Situacao = SituacaoViagemEnum.Agendada;
            Ativo = ativo;
            
            // Inicializando campos obrigatórios
            MotivoProblema = string.Empty;
            LatitudeFimViagem = 0;
            LatitudeInicioViagem = 0;
            DistanciaRealizada = 0;
            PolilinhaRotaRealizada = string.Empty;
            MotivoCancelamento = string.Empty;
        }

        public string CodigoViagem { get; private set; }
        public DateTime DataViagem { get; private set; }
        public TimeSpan HorarioSaida { get; private set; }
        public TimeSpan HorarioChegada { get; private set; }
        public virtual Veiculo? Veiculo { get; private set; }
        public long VeiculoId { get; private set; }
        public virtual Motorista? Motorista { get; private set; }
        public long MotoristaId { get; private set; }
        public virtual Localidade? LocalidadeOrigem { get; private set; }
        public long LocalidadeOrigemId { get; private set; }
        public virtual Localidade? LocalidadeDestino { get; private set; }
        public long LocalidadeDestinoId { get; private set; }
        public virtual GatilhoViagem? GatilhoViagem { get; private set; }
        public long? GatilhoViagemId { get; private set; }
        public decimal ValorPassagem { get; private set; }
        public int QuantidadeVagas { get; private set; }
        public int VagasDisponiveis { get; private set; }
        public decimal Distancia { get; private set; }
        public int NumeroPassageiros { get; private set; }
        public bool Lotado { get; private set; }
        public bool Excesso { get; private set; }
        public string MotivoProblema { get; private set; }
        public string DescricaoViagem { get; private set; }
        public decimal LatitudeFimViagem { get; private set; }
        public decimal LatitudeInicioViagem { get; private set; }
        public decimal DistanciaRealizada { get; private set; }
        public string PolilinhaRota { get; private set; }
        public string PolilinhaRotaRealizada { get; private set; }
        public SituacaoViagemEnum Situacao { get; private set; }
        public string MotivoCancelamento { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime? DataInicioViagem { get; private set; }
        public DateTime? DataFimViagem { get; private set; }

        public void Atualizar(
            DateTime dataViagem,
            TimeSpan horaSaida,
            TimeSpan horaChegada,
            long veiculoId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            decimal valorPassagem,
            int quantidadeVagas,
            bool ativo)
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Somente viagens agendadas podem ser editadas");

            DataViagem = dataViagem;
            HorarioSaida = horaSaida;
            HorarioChegada = horaChegada;
            VeiculoId = veiculoId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            ValorPassagem = valorPassagem;
            
            if (quantidadeVagas < (QuantidadeVagas - VagasDisponiveis))
                throw new DomainException("Não é possível reduzir a quantidade de vagas abaixo do número de passageiros já reservados");
                
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas - (QuantidadeVagas - VagasDisponiveis);
            Ativo = ativo;
        }

        public void Cancelar(string motivo)
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Somente viagens agendadas podem ser canceladas");

            if (string.IsNullOrEmpty(motivo))
                throw new DomainException("O motivo do cancelamento é obrigatório");

            Situacao = SituacaoViagemEnum.Cancelada;
            MotivoCancelamento = motivo;
            Ativo = false;
        }

        public void IniciarViagem()
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Somente viagens agendadas podem ser iniciadas");

            Situacao = SituacaoViagemEnum.EmAndamento;
            DataInicioViagem = DateTime.Now;
        }

        public void FinalizarViagem()
        {
            if (Situacao != SituacaoViagemEnum.EmAndamento)
                throw new DomainException("Somente viagens em andamento podem ser finalizadas");

            Situacao = SituacaoViagemEnum.Finalizada;
            DataFimViagem = DateTime.Now;
        }

        public void ReservarVaga()
        {
            if (VagasDisponiveis <= 0)
                throw new DomainException("Não há vagas disponíveis para esta viagem");

            VagasDisponiveis--;
            NumeroPassageiros++;
            Lotado = VagasDisponiveis == 0;
        }

        public void LiberarVaga()
        {
            if (VagasDisponiveis >= QuantidadeVagas)
                throw new DomainException("Todas as vagas já estão disponíveis");

            VagasDisponiveis++;
            NumeroPassageiros--;
            Lotado = false;
        }

        private string GerarCodigoViagem()
        {
            return $"VIA{DateTime.Now:yyyyMMddHHmmss}";
        }

        protected override string DescricaoFormatada => CodigoViagem;
    }
}
