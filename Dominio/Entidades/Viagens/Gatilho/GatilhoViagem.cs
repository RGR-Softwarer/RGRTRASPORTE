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
            if (string.IsNullOrEmpty(descricao))
                throw new DomainException("A descrição é obrigatória");

            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

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

        public void Atualizar(
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
            if (string.IsNullOrEmpty(descricao))
                throw new DomainException("A descrição é obrigatória");

            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

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
                Ativo);
        }

        protected override string DescricaoFormatada => Descricao;
    }
}
