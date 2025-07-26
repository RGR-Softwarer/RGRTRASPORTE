using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Data;
using Dominio.Enums.Viagens;
using Dominio.Exceptions;

namespace Dominio.Entidades.Viagens.Gatilho
{
    public class GatilhoViagem : BaseEntity
    {
        protected GatilhoViagem() { } // Construtor protegido para EF Core

        public GatilhoViagem(
            string descricao,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            decimal valorPassagem,
            int quantidadeVagas,
            decimal distancia,
            string descricaoViagem,
            string polilinhaRota,
            List<DiaSemanaEnum> diasSemana,
            bool ativo)
        {
            ValidarCriacao(descricao, horarioSaida, horarioChegada, veiculoId, motoristaId,
                localidadeOrigemId, localidadeDestinoId, valorPassagem, quantidadeVagas,
                distancia, descricaoViagem, polilinhaRota, diasSemana);

            Descricao = descricao;
            VeiculoId = veiculoId;
            MotoristaId = motoristaId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            HorarioSaida = horarioSaida;
            HorarioChegada = horarioChegada;
            ValorPassagem = valorPassagem;
            QuantidadeVagas = quantidadeVagas;
            Distancia = distancia;
            DescricaoViagem = descricaoViagem;
            PolilinhaRota = polilinhaRota;
            DiasSemana = diasSemana;
            Ativo = ativo;
        }

        public string Descricao { get; private set; }
        public virtual Veiculo Veiculo { get; private set; }
        public long VeiculoId { get; private set; }
        public long MotoristaId { get; private set; }
        public virtual Localidade LocalidadeOrigem { get; private set; }
        public long LocalidadeOrigemId { get; private set; }
        public virtual Localidade LocalidadeDestino { get; private set; }
        public long LocalidadeDestinoId { get; private set; }
        public TimeSpan HorarioSaida { get; private set; }
        public TimeSpan HorarioChegada { get; private set; }
        public decimal ValorPassagem { get; private set; }
        public int QuantidadeVagas { get; private set; }
        public decimal Distancia { get; private set; }
        public string DescricaoViagem { get; private set; }
        public string PolilinhaRota { get; private set; }
        public bool Ativo { get; private set; }
        public virtual ICollection<Viagem> Viagens { get; private set; }
        public List<DiaSemanaEnum> DiasSemana { get; private set; }

        private void ValidarCriacao(
            string descricao,
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
            List<DiaSemanaEnum> diasSemana)
        {
            if (string.IsNullOrEmpty(descricao))
                throw new DomainException("A descrição é obrigatória");

            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

            if (horarioChegada <= horarioSaida)
                throw new DomainException("O horário de chegada deve ser maior que o horário de saída");

            if (veiculoId <= 0)
                throw new DomainException("O veículo é obrigatório");

            if (motoristaId <= 0)
                throw new DomainException("O motorista é obrigatório");

            if (localidadeOrigemId <= 0)
                throw new DomainException("A localidade de origem é obrigatória");

            if (localidadeDestinoId <= 0)
                throw new DomainException("A localidade de destino é obrigatória");

            if (localidadeOrigemId == localidadeDestinoId)
                throw new DomainException("A localidade de destino não pode ser igual à localidade de origem");

            if (valorPassagem <= 0)
                throw new DomainException("O valor da passagem deve ser maior que zero");

            if (quantidadeVagas <= 0)
                throw new DomainException("A quantidade de vagas deve ser maior que zero");

            if (distancia <= 0)
                throw new DomainException("A distância deve ser maior que zero");

            if (string.IsNullOrEmpty(descricaoViagem))
                throw new DomainException("A descrição da viagem é obrigatória");

            if (descricaoViagem.Length > 500)
                throw new DomainException("A descrição da viagem não pode ter mais que 500 caracteres");

            if (string.IsNullOrEmpty(polilinhaRota))
                throw new DomainException("A polilinha da rota é obrigatória");
        }

        public void AtualizarHorarios(TimeSpan horarioSaida, TimeSpan horarioChegada)
        {
            if (horarioChegada <= horarioSaida)
                throw new DomainException("O horário de chegada deve ser maior que o horário de saída");

            HorarioSaida = horarioSaida;
            HorarioChegada = horarioChegada;
        }

        public void AtualizarValorPassagem(decimal valorPassagem)
        {
            if (valorPassagem <= 0)
                throw new DomainException("O valor da passagem deve ser maior que zero");

            ValorPassagem = valorPassagem;
        }

        public void AtualizarQuantidadeVagas(int quantidadeVagas)
        {
            if (quantidadeVagas <= 0)
                throw new DomainException("A quantidade de vagas deve ser maior que zero");

            QuantidadeVagas = quantidadeVagas;
        }

        public void AtualizarDiasSemana(List<DiaSemanaEnum> diasSemana)
        {
            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

            DiasSemana = diasSemana;
        }

        public void Ativar()
        {
            Ativo = true;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public Viagem GerarViagem(DateTime data)
        {
            if (!DiasSemana.Contains((DiaSemanaEnum)data.DayOfWeek))
                throw new DomainException("Data não corresponde aos dias da semana configurados");

            return new Viagem(
                data,
                HorarioSaida,
                HorarioChegada,
                VeiculoId,
                MotoristaId,
                LocalidadeOrigemId,
                LocalidadeDestinoId,
                ValorPassagem,
                QuantidadeVagas,
                Distancia,
                DescricaoViagem,
                PolilinhaRota,
                Ativo,
                Id);
        }

        protected override string DescricaoFormatada => Descricao;
    }
}
