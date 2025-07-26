#nullable enable

using Dominio.Entidades.Localidades;
using Dominio.Entidades.Pessoas;
using Dominio.Entidades.Veiculos;
using Dominio.Entidades.Viagens.Gatilho;
using Dominio.Entidades.Pessoas.Passageiros;
using Dominio.Enums.Viagens;
using Dominio.Exceptions;
using Dominio.ValueObjects;
using Dominio.Events;
using System.Collections.Generic;
using Dominio.Events.Base;
using Dominio.Entidades;
using Dominio.Events.Viagens;


namespace Dominio.Entidades.Viagens
{
    public class Viagem : BaseEntity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public string Codigo { get; private set; }
        public long VeiculoId { get; private set; }
        public long MotoristaId { get; private set; }
        public long LocalidadeOrigemId { get; private set; }
        public long LocalidadeDestinoId { get; private set; }
        public DateTime DataViagem { get; private set; }
        public TimeSpan HorarioSaida { get; private set; }
        public TimeSpan HorarioChegada { get; private set; }
        public SituacaoViagemEnum Situacao { get; private set; }
        public bool Lotado { get; private set; }
        public DateTime? DataInicioViagem { get; private set; }
        public DateTime? DataFimViagem { get; private set; }
        public decimal ValorPassagem { get; private set; }
        public int QuantidadeVagas { get; private set; }
        public int VagasDisponiveis { get; private set; }
        public decimal Distancia { get; private set; }
        public string DescricaoViagem { get; private set; }
        public string PolilinhaRota { get; private set; }
        public bool Ativo { get; private set; }
        public long? GatilhoViagemId { get; private set; }

        // Entidades filhas
        private readonly List<ViagemPassageiro> _passageiros = new();
        public IReadOnlyCollection<ViagemPassageiro> Passageiros => _passageiros.AsReadOnly();

        private readonly List<ViagemPosicao> _posicoes = new();
        public IReadOnlyCollection<ViagemPosicao> Posicoes => _posicoes.AsReadOnly();

        // Navegação
        public Veiculo Veiculo { get; private set; }
        public Motorista Motorista { get; private set; }
        public Localidade LocalidadeOrigem { get; private set; }
        public Localidade LocalidadeDestino { get; private set; }
        public GatilhoViagem GatilhoViagem { get; private set; }

        private Viagem() { } // Para EF

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
            bool ativo,
            long? gatilhoViagemId = null)
        {
            ValidarCriacao(dataViagem, horarioSaida, horarioChegada, veiculoId, motoristaId, 
                localidadeOrigemId, localidadeDestinoId, valorPassagem, quantidadeVagas, 
                distancia, descricaoViagem, polilinhaRota);

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
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;
            Situacao = SituacaoViagemEnum.Agendada;
            Codigo = GerarCodigoViagem();

            AddDomainEvent(new ViagemCriadaEvent(Id, veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId));
        }

        private void ValidarCriacao(
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
            string polilinhaRota)
        {
            if (dataViagem.Date < DateTime.Today)
                throw new DomainException("A data da viagem deve ser maior ou igual à data atual");

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

        public void AdicionarPassageiro(Passageiro passageiro)
        {
            ValidarAdicaoPassageiro(passageiro);
            var viagemPassageiro = new ViagemPassageiro(this, passageiro.Id);
            _passageiros.Add(viagemPassageiro);
            VagasDisponiveis--;
            AtualizarStatusLotacao();
            AddDomainEvent(new PassageiroAdicionadoEvent(Id, passageiro.Id));
        }

        public void RemoverPassageiro(long passageiroId)
        {
            var viagemPassageiro = _passageiros.FirstOrDefault(p => p.PassageiroId == passageiroId);
            if (viagemPassageiro == null)
                throw new DomainException("Passageiro não encontrado na viagem");

            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Apenas viagens agendadas podem ter passageiros removidos");

            _passageiros.Remove(viagemPassageiro);
            VagasDisponiveis++;
            AtualizarStatusLotacao();
            AddDomainEvent(new PassageiroRemovidoEvent(Id, passageiroId));
        }

        public void AdicionarPosicao(decimal latitude, decimal longitude, DateTime dataHora)
        {
            ValidarAdicaoPosicao(latitude, longitude, dataHora);
            var posicao = new ViagemPosicao(this, latitude, longitude, dataHora);
            _posicoes.Add(posicao);
            AddDomainEvent(new PosicaoAdicionadaEvent(Id, latitude, longitude, dataHora));
        }

        private void ValidarAdicaoPosicao(decimal latitude, decimal longitude, DateTime dataHora)
        {
            if (Situacao != SituacaoViagemEnum.EmAndamento)
                throw new DomainException("Apenas viagens em andamento podem receber posições");

            if (latitude < -90 || latitude > 90)
                throw new DomainException("Latitude inválida");

            if (longitude < -180 || longitude > 180)
                throw new DomainException("Longitude inválida");

            if (dataHora > DateTime.UtcNow)
                throw new DomainException("Data/hora não pode ser futura");
        }

        private void ValidarAdicaoPassageiro(Passageiro passageiro)
        {
            if (passageiro == null)
                throw new DomainException("Passageiro é obrigatório");

            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Apenas viagens agendadas podem receber passageiros");

            if (VagasDisponiveis <= 0)
                throw new DomainException("Não há vagas disponíveis");

            if (_passageiros.Any(p => p.PassageiroId == passageiro.Id))
                throw new DomainException("Passageiro já está na viagem");
        }

        public void IniciarViagem()
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Apenas viagens agendadas podem ser iniciadas");

            Situacao = SituacaoViagemEnum.EmAndamento;
            DataInicioViagem = DateTime.UtcNow;
            AddDomainEvent(new ViagemIniciadaEvent(Id, DataInicioViagem.Value));
        }

        public void FinalizarViagem()
        {
            if (Situacao != SituacaoViagemEnum.EmAndamento)
                throw new DomainException("Apenas viagens em andamento podem ser finalizadas");

            Situacao = SituacaoViagemEnum.Finalizada;
            DataFimViagem = DateTime.UtcNow;
            AddDomainEvent(new ViagemFinalizadaEvent(Id, DataFimViagem.Value));
        }

        public void CancelarViagem(string motivo)
        {
            if (Situacao == SituacaoViagemEnum.Finalizada)
                throw new DomainException("Viagens finalizadas não podem ser canceladas");

            Situacao = SituacaoViagemEnum.Cancelada;
            AddDomainEvent(new ViagemCanceladaEvent(Id, motivo));
        }

        private void AtualizarStatusLotacao()
        {
            Lotado = VagasDisponiveis <= 0;
        }

        private void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        private string GerarCodigoViagem()
        {
            return $"VIA{DateTime.UtcNow:yyyyMMddHHmmss}";
        }

        public void Atualizar(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            decimal valorPassagem,
            int quantidadeVagas,
            bool ativo,
            long? gatilhoViagemId = null)
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Apenas viagens agendadas podem ser editadas");

            ValidarCriacao(dataViagem, horarioSaida, horarioChegada, veiculoId, MotoristaId,
                localidadeOrigemId, localidadeDestinoId, valorPassagem, quantidadeVagas,
                Distancia, DescricaoViagem, PolilinhaRota);

            DataViagem = dataViagem;
            HorarioSaida = horarioSaida;
            HorarioChegada = horarioChegada;
            VeiculoId = veiculoId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            ValorPassagem = valorPassagem;
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas - _passageiros.Count;
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;

            AddDomainEvent(new ViagemAtualizadaEvent(Id));
        }
    }
}
