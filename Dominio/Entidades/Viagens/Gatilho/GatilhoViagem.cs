using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Enums.Data;
using Dominio.Enums.Viagens;
using Dominio.Events.Base;
using Dominio.Exceptions;
using Dominio.Interfaces;
using Dominio.Services;

namespace Dominio.Entidades.Viagens.Gatilho
{
    public class GatilhoViagem : AggregateRoot
    {
        private GatilhoViagem() { } // Para EF Core

        // Construtor privado - usar Factory Methods
        private GatilhoViagem(
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

            AddDomainEvent(new GatilhoViagemCriadoEvent(Id, descricao));
        }

        // Factory Methods
        public static GatilhoViagem CriarGatilhoViagem(
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
            List<DiaSemanaEnum> diasSemana)
        {
            return new GatilhoViagem(descricao, veiculoId, motoristaId, localidadeOrigemId,
                localidadeDestinoId, horarioSaida, horarioChegada, valorPassagem, quantidadeVagas,
                distancia, descricaoViagem, polilinhaRota, diasSemana, true);
        }

        // Factory Method com validação por NotificationContext
        public static (GatilhoViagem? gatilho, bool sucesso) CriarGatilhoViagemComValidacao(
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
            IDomainNotificationContext notificationContext)
        {
            var validationService = new GatilhoViagemValidationService();
            var valido = validationService.ValidarCriacao(descricao, veiculoId, motoristaId,
                localidadeOrigemId, localidadeDestinoId, horarioSaida, horarioChegada, valorPassagem,
                quantidadeVagas, distancia, descricaoViagem, polilinhaRota, diasSemana, notificationContext);

            if (!valido)
                return (null, false);

            try
            {
                var gatilho = CriarGatilhoViagem(descricao, veiculoId, motoristaId, localidadeOrigemId,
                    localidadeDestinoId, horarioSaida, horarioChegada, valorPassagem, quantidadeVagas,
                    distancia, descricaoViagem, polilinhaRota, diasSemana);
                
                return (gatilho, true);
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return (null, false);
            }
        }

        public string Descricao { get; private set; } = null!;
        public virtual Veiculo Veiculo { get; private set; } = null!;
        public long VeiculoId { get; private set; }
        public long MotoristaId { get; private set; }
        public virtual Localidade LocalidadeOrigem { get; private set; } = null!;
        public long LocalidadeOrigemId { get; private set; }
        public virtual Localidade LocalidadeDestino { get; private set; } = null!;
        public long LocalidadeDestinoId { get; private set; }
        public TimeSpan HorarioSaida { get; private set; }
        public TimeSpan HorarioChegada { get; private set; }
        public decimal ValorPassagem { get; private set; }
        public int QuantidadeVagas { get; private set; }
        public decimal Distancia { get; private set; }
        public string DescricaoViagem { get; private set; } = null!;
        public string PolilinhaRota { get; private set; } = null!;
        public bool Ativo { get; private set; }
        public virtual ICollection<Viagem> Viagens { get; private set; } = null!;
        public List<DiaSemanaEnum> DiasSemana { get; private set; } = null!;

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
                throw new DomainException("A descri��o � obrigat�ria");

            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

            if (horarioChegada <= horarioSaida)
                throw new DomainException("O hor�rio de chegada deve ser maior que o hor�rio de sa�da");

            if (veiculoId <= 0)
                throw new DomainException("O ve�culo � obrigat�rio");

            if (motoristaId <= 0)
                throw new DomainException("O motorista � obrigat�rio");

            if (localidadeOrigemId <= 0)
                throw new DomainException("A localidade de origem � obrigat�ria");

            if (localidadeDestinoId <= 0)
                throw new DomainException("A localidade de destino � obrigat�ria");

            if (localidadeOrigemId == localidadeDestinoId)
                throw new DomainException("A localidade de destino n�o pode ser igual � localidade de origem");

            if (valorPassagem <= 0)
                throw new DomainException("O valor da passagem deve ser maior que zero");

            if (quantidadeVagas <= 0)
                throw new DomainException("A quantidade de vagas deve ser maior que zero");

            if (distancia <= 0)
                throw new DomainException("A dist�ncia deve ser maior que zero");

            if (string.IsNullOrEmpty(descricaoViagem))
                throw new DomainException("A descri��o da viagem � obrigat�ria");

            if (descricaoViagem.Length > 500)
                throw new DomainException("A descri��o da viagem n�o pode ter mais que 500 caracteres");

            if (string.IsNullOrEmpty(polilinhaRota))
                throw new DomainException("A polilinha da rota � obrigat�ria");
        }

        public void AtualizarHorarios(TimeSpan horarioSaida, TimeSpan horarioChegada)
        {
            if (horarioChegada <= horarioSaida)
                throw new DomainException("O hor�rio de chegada deve ser maior que o hor�rio de sa�da");

            HorarioSaida = horarioSaida;
            HorarioChegada = horarioChegada;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemHorarioAtualizadoEvent(Id, Descricao));
        }

        public void AtualizarValorPassagem(decimal valorPassagem)
        {
            if (valorPassagem <= 0)
                throw new DomainException("O valor da passagem deve ser maior que zero");

            ValorPassagem = valorPassagem;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemValorAtualizadoEvent(Id, Descricao, valorPassagem));
        }

        public void AtualizarQuantidadeVagas(int quantidadeVagas)
        {
            if (quantidadeVagas <= 0)
                throw new DomainException("A quantidade de vagas deve ser maior que zero");

            QuantidadeVagas = quantidadeVagas;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemVagasAtualizadasEvent(Id, Descricao, quantidadeVagas));
        }

        public void AtualizarDiasSemana(List<DiaSemanaEnum> diasSemana)
        {
            if (diasSemana == null || !diasSemana.Any())
                throw new DomainException("Pelo menos um dia da semana deve ser selecionado");

            DiasSemana = diasSemana;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemDiasAtualizadosEvent(Id, Descricao));
        }

        public void Ativar()
        {
            Ativo = true;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemAtivadoEvent(Id, Descricao));
        }

        public void Desativar()
        {
            Ativo = false;
            UpdateTimestamp();
            AddDomainEvent(new GatilhoViagemDesativadoEvent(Id, Descricao));
        }

        public Viagem GerarViagem(DateTime data)
        {
            if (!DiasSemana.Contains((DiaSemanaEnum)data.DayOfWeek))
                throw new DomainException("Data n�o corresponde aos dias da semana configurados");

            var viagem = Viagem.CriarViagemComGatilho(
                data,
                this,
                VeiculoId,
                MotoristaId,
                QuantidadeVagas,
                Distancia,
                DescricaoViagem,
                PolilinhaRota,
                Ativo);

            AddDomainEvent(new ViagemGeradaPorGatilhoEvent(Id, viagem.Id, data));
            return viagem;
        }

        protected override string DescricaoFormatada => Descricao;
    }

    // Eventos de dom�nio para GatilhoViagem
    public class GatilhoViagemCriadoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }

        public GatilhoViagemCriadoEvent(long gatilhoId, string descricao)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
        }
    }

    public class GatilhoViagemHorarioAtualizadoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }

        public GatilhoViagemHorarioAtualizadoEvent(long gatilhoId, string descricao)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
        }
    }

    public class GatilhoViagemValorAtualizadoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }
        public decimal NovoValor { get; }

        public GatilhoViagemValorAtualizadoEvent(long gatilhoId, string descricao, decimal novoValor)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
            NovoValor = novoValor;
        }
    }

    public class GatilhoViagemVagasAtualizadasEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }
        public int NovaQuantidade { get; }

        public GatilhoViagemVagasAtualizadasEvent(long gatilhoId, string descricao, int novaQuantidade)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
            NovaQuantidade = novaQuantidade;
        }
    }

    public class GatilhoViagemDiasAtualizadosEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }

        public GatilhoViagemDiasAtualizadosEvent(long gatilhoId, string descricao)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
        }
    }

    public class GatilhoViagemAtivadoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }

        public GatilhoViagemAtivadoEvent(long gatilhoId, string descricao)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
        }
    }

    public class GatilhoViagemDesativadoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public string Descricao { get; }

        public GatilhoViagemDesativadoEvent(long gatilhoId, string descricao)
        {
            GatilhoId = gatilhoId;
            Descricao = descricao;
        }
    }

    public class ViagemGeradaPorGatilhoEvent : DomainEvent
    {
        public long GatilhoId { get; }
        public long ViagemId { get; }
        public DateTime DataViagem { get; }

        public ViagemGeradaPorGatilhoEvent(long gatilhoId, long viagemId, DateTime dataViagem)
        {
            GatilhoId = gatilhoId;
            ViagemId = viagemId;
            DataViagem = dataViagem;
        }
    }
}
