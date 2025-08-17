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
using Dominio.Specifications;
using Dominio.Services;
using Dominio.Interfaces;

namespace Dominio.Entidades.Viagens
{
    /// <summary>
    /// Aggregate Root para entidade Viagem seguindo padrões DDD.
    /// Representa uma viagem de transporte com passageiros, rotas e posicionamento.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa:
    /// - Factory Methods para criação controlada
    /// - Value Objects para tipos complexos
    /// - Domain Events para comunicação
    /// - Specifications para regras de negócio
    /// - Notification Pattern para validações
    /// </remarks>
    public class Viagem : AggregateRoot
    {
        // Value Objects
        public CodigoViagem Codigo { get; private set; }
        public PeriodoViagem Periodo { get; private set; }
        public Distancia Distancia { get; private set; }
        public Polilinha PolilinhaRota { get; private set; }

        // Propriedades primitivas que não justificam Value Objects
        public long VeiculoId { get; private set; }
        public long MotoristaId { get; private set; }
        public long LocalidadeOrigemId { get; private set; }
        public long LocalidadeDestinoId { get; private set; }
        public SituacaoViagemEnum Situacao { get; private set; }
        public bool Lotado { get; private set; }
        public DateTime? DataInicioViagem { get; private set; }
        public DateTime? DataFimViagem { get; private set; }
        public int QuantidadeVagas { get; private set; }
        public int VagasDisponiveis { get; private set; }
        public string DescricaoViagem { get; private set; }
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

        // Specifications para consultas básicas
        private static readonly ViagemPodeSerIniciadaSpecification _podeSerIniciada = new();
        private static readonly ViagemPodeSerFinalizadaSpecification _podeSerFinalizada = new();
        private static readonly ViagemPodeSerCanceladaSpecification _podeSerCancelada = new();
        private static readonly ViagemPodeSerEditadaSpecification _podeSerEditada = new();
        private static readonly ViagemPodeReceberPosicaoSpecification _podeReceberPosicao = new();

        private Viagem() { } // Para EF

        // Construtor privado - usar Factory Methods
        private Viagem(
            PeriodoViagem periodo,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            Distancia distancia,
            string descricaoViagem,
            Polilinha polilinhaRota,
            bool ativo,
            long? gatilhoViagemId = null)
        {
            // Validações básicas de integridade - as regras de negócio complexas são validadas no ValidationService
            if (veiculoId <= 0 || motoristaId <= 0 || localidadeOrigemId <= 0 || localidadeDestinoId <= 0 || quantidadeVagas <= 0)
                throw new DomainException("Dados obrigatórios não informados");

            Periodo = periodo;
            VeiculoId = veiculoId;
            MotoristaId = motoristaId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas;
            Distancia = distancia;
            DescricaoViagem = descricaoViagem;
            PolilinhaRota = polilinhaRota;
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;
            Situacao = SituacaoViagemEnum.Agendada;
            Codigo = CodigoViagem.Gerar();

            AddDomainEvent(new ViagemCriadaEvent(Id, veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId));
        }

        // Factory Methods
        /// <summary>
        /// Cria uma nova viagem regular através de Factory Method.
        /// </summary>
        /// <param name="dataViagem">Data da viagem</param>
        /// <param name="horarioSaida">Horário de saída</param>
        /// <param name="horarioChegada">Horário de chegada</param>
        /// <param name="veiculoId">ID do veículo</param>
        /// <param name="motoristaId">ID do motorista</param>
        /// <param name="localidadeOrigemId">ID da localidade de origem</param>
        /// <param name="localidadeDestinoId">ID da localidade de destino</param>
        /// <param name="quantidadeVagas">Quantidade de vagas disponíveis</param>
        /// <param name="distanciaKm">Distância em quilômetros</param>
        /// <param name="descricaoViagem">Descrição da viagem</param>
        /// <param name="polilinhaRota">Polilinha da rota</param>
        /// <param name="ativo">Se a viagem está ativa</param>
        /// <returns>Nova instância de Viagem</returns>
        /// <exception cref="DomainException">Lançada quando dados obrigatórios não são informados</exception>
        public static Viagem CriarViagemRegular(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            decimal distanciaKm,
            string descricaoViagem,
            string polilinhaRota,
            bool ativo = true)
        {
            var periodo = new PeriodoViagem(dataViagem, horarioSaida, horarioChegada);
            var distancia = new Distancia(distanciaKm);
            var polilinha = new Polilinha(polilinhaRota);

            return new Viagem(periodo, veiculoId, motoristaId, localidadeOrigemId, 
                localidadeDestinoId, quantidadeVagas, distancia, descricaoViagem, 
                polilinha, ativo);
        }

        // Factory Method com validação por NotificationContext
        public static (Viagem? viagem, bool sucesso) CriarViagemRegularComValidacao(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long motoristaId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            decimal distanciaKm,
            string descricaoViagem,
            string polilinhaRota,
            IDomainNotificationContext notificationContext,
            bool ativo = true)
        {
            var validationService = new ViagemValidationService();
            var valido = validationService.ValidarCriacao(
                dataViagem, horarioSaida, horarioChegada, veiculoId, motoristaId,
                localidadeOrigemId, localidadeDestinoId, quantidadeVagas,
                distanciaKm, descricaoViagem, polilinhaRota, notificationContext);

            if (!valido)
                return (null, false);

            try
            {
                var viagem = CriarViagemRegular(dataViagem, horarioSaida, horarioChegada,
                    veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId,
                    quantidadeVagas, distanciaKm, descricaoViagem, polilinhaRota, ativo);
                
                return (viagem, true);
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return (null, false);
            }
        }

        public static Viagem CriarViagemComGatilho(
            DateTime dataViagem,
            GatilhoViagem gatilho,
            long veiculoId,
            long motoristaId,
            int quantidadeVagas,
            decimal distanciaKm,
            string descricaoViagem,
            string polilinhaRota,
            bool ativo = true)
        {
            var periodo = new PeriodoViagem(dataViagem, gatilho.HorarioSaida, gatilho.HorarioChegada);
            var distancia = new Distancia(distanciaKm);
            var polilinha = new Polilinha(polilinhaRota);

            return new Viagem(periodo, veiculoId, motoristaId, 
                gatilho.LocalidadeOrigemId, gatilho.LocalidadeDestinoId, 
                quantidadeVagas, distancia, descricaoViagem, polilinha, 
                ativo, gatilho.Id);
        }

        // Métodos de negócio
        public void AdicionarPassageiro(Passageiro passageiro)
        {
            EnsurePassageiroValido(passageiro);
            EnsureViagemPodeReceberPassageiros();
            EnsurePassageiroNaoEstaNaviagem(passageiro.Id);

            var viagemPassageiro = new ViagemPassageiro(this, passageiro.Id);
            _passageiros.Add(viagemPassageiro);
            
            AtualizarVagasDisponiveis();
            UpdateTimestamp();
            
            AddDomainEvent(new PassageiroAdicionadoEvent(Id, passageiro.Id));
            
            if (Lotado)
                AddDomainEvent(new ViagemLotadaEvent(Id, _passageiros.Count));
        }

        // Método de negócio com validação por NotificationContext
        public bool AdicionarPassageiroComValidacao(Passageiro passageiro, IDomainNotificationContext notificationContext)
        {
            var validationService = new ViagemValidationService();
            var valido = validationService.ValidarAdicaoPassageiro(this, passageiro, notificationContext);

            if (!valido)
                return false;

            try
            {
                AdicionarPassageiro(passageiro);
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        public void RemoverPassageiro(long passageiroId)
        {
            var viagemPassageiro = ObterPassageiro(passageiroId);
            EnsurePassageiroExiste(viagemPassageiro);
            EnsureViagemPodeReceberPassageiros();

            var estavaLotado = Lotado;
            _passageiros.Remove(viagemPassageiro);
            
            AtualizarVagasDisponiveis();
            UpdateTimestamp();
            
            AddDomainEvent(new PassageiroRemovidoEvent(Id, passageiroId));
            
            if (estavaLotado && !Lotado)
                AddDomainEvent(new ViagemComVagasDisponiveisEvent(Id, VagasDisponiveis));
        }

        public void AdicionarPosicao(decimal latitude, decimal longitude, DateTime dataHora)
        {
            EnsureViagemPodeReceberPosicoes();
            ValidarDataHoraPosicao(dataHora);

            var coordenada = new Coordenada(latitude, longitude);
            var posicao = new ViagemPosicao(this, coordenada.Latitude, coordenada.Longitude, dataHora);
            _posicoes.Add(posicao);
            
            UpdateTimestamp();
            AddDomainEvent(new PosicaoAdicionadaEvent(Id, latitude, longitude, dataHora));
        }

        // Método de negócio com validação por NotificationContext
        public bool AdicionarPosicaoComValidacao(decimal latitude, decimal longitude, DateTime dataHora, IDomainNotificationContext notificationContext)
        {
            var validationService = new ViagemValidationService();
            var valido = validationService.ValidarAdicaoPosicao(this, latitude, longitude, dataHora, notificationContext);

            if (!valido)
                return false;

            try
            {
                AdicionarPosicao(latitude, longitude, dataHora);
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        public void IniciarViagem()
        {
            EnsureViagemPodeSerIniciada();

            Situacao = SituacaoViagemEnum.EmAndamento;
            DataInicioViagem = DateTime.UtcNow;
            UpdateTimestamp();
            
            AddDomainEvent(new ViagemIniciadaEvent(Id, DataInicioViagem.Value));
        }

        public void FinalizarViagem()
        {
            EnsureViagemPodeSerFinalizada();

            Situacao = SituacaoViagemEnum.Finalizada;
            DataFimViagem = DateTime.UtcNow;
            UpdateTimestamp();
            
            AddDomainEvent(new ViagemFinalizadaEvent(Id, DataFimViagem.Value));
        }

        public void CancelarViagem(string motivo)
        {
            EnsureViagemPodeSerCancelada();

            Situacao = SituacaoViagemEnum.Cancelada;
            UpdateTimestamp();
            
            AddDomainEvent(new ViagemCanceladaEvent(Id, motivo));
        }

        public void Atualizar(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            bool ativo,
            long? gatilhoViagemId = null)
        {
            EnsureViagemPodeSerEditada();

            var horariosAnteriores = (Periodo.HoraSaida, Periodo.HoraChegada);
            var novosPeriodo = new PeriodoViagem(dataViagem, horarioSaida, horarioChegada);

            // Validações básicas de integridade
            if (veiculoId <= 0 || localidadeOrigemId <= 0 || localidadeDestinoId <= 0 || quantidadeVagas <= 0)
                throw new DomainException("Dados obrigatórios não informados");

            Periodo = novosPeriodo;
            VeiculoId = veiculoId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas - _passageiros.Count;
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;
            
            UpdateTimestamp();

            AddDomainEvent(new ViagemAtualizadaEvent(Id));

            if (horariosAnteriores.HoraSaida != horarioSaida || horariosAnteriores.HoraChegada != horarioChegada)
            {
                AddDomainEvent(new HorarioViagemAlteradoEvent(Id, 
                    horariosAnteriores.HoraSaida, horariosAnteriores.HoraChegada,
                    horarioSaida, horarioChegada));
            }
        }

        // Método de atualização com validação por NotificationContext
        public bool AtualizarComValidacao(
            DateTime dataViagem,
            TimeSpan horarioSaida,
            TimeSpan horarioChegada,
            long veiculoId,
            long localidadeOrigemId,
            long localidadeDestinoId,
            int quantidadeVagas,
            bool ativo,
            IDomainNotificationContext notificationContext,
            long? gatilhoViagemId = null)
        {
            var validationService = new ViagemValidationService();
            var valido = validationService.ValidarAtualizacao(this, dataViagem, horarioSaida, horarioChegada,
                veiculoId, localidadeOrigemId, localidadeDestinoId, quantidadeVagas, notificationContext);

            if (!valido)
                return false;

            try
            {
                Atualizar(dataViagem, horarioSaida, horarioChegada, veiculoId,
                    localidadeOrigemId, localidadeDestinoId, quantidadeVagas, ativo, gatilhoViagemId);
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        // Métodos de consulta
        public bool TemPassageiro(long passageiroId) 
            => _passageiros.Any(p => p.PassageiroId == passageiroId);

        public int QuantidadePassageiros => _passageiros.Count;

        public ViagemPassageiro? ObterPassageiro(long passageiroId) 
            => _passageiros.FirstOrDefault(p => p.PassageiroId == passageiroId);

        public bool PodeReceberPassageiros() => Situacao == SituacaoViagemEnum.Agendada && VagasDisponiveis > 0;
        public bool PodeSerIniciada() => _podeSerIniciada.IsSatisfiedBy(this);
        public bool PodeSerFinalizada() => _podeSerFinalizada.IsSatisfiedBy(this);
        public bool PodeSerCancelada() => _podeSerCancelada.IsSatisfiedBy(this);

        // Validações usando Specifications
        private void EnsureViagemPodeReceberPassageiros()
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Viagem deve estar agendada para receber passageiros");
            
            if (VagasDisponiveis <= 0)
                throw new DomainException("Não há vagas disponíveis na viagem");
        }

        private void EnsureViagemPodeSerIniciada()
        {
            if (!_podeSerIniciada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerIniciada.ErrorMessage);
        }

        private void EnsureViagemPodeSerFinalizada()
        {
            if (!_podeSerFinalizada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerFinalizada.ErrorMessage);
        }

        private void EnsureViagemPodeSerCancelada()
        {
            if (!_podeSerCancelada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerCancelada.ErrorMessage);
        }

        private void EnsureViagemPodeSerEditada()
        {
            if (!_podeSerEditada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerEditada.ErrorMessage);
        }

        private void EnsureViagemPodeReceberPosicoes()
        {
            if (!_podeReceberPosicao.IsSatisfiedBy(this))
                throw new DomainException(_podeReceberPosicao.ErrorMessage);
        }

        private void EnsurePassageiroValido(Passageiro passageiro)
        {
            if (passageiro == null)
                throw new DomainException("Passageiro é obrigatório");
        }

        private void EnsurePassageiroNaoEstaNaviagem(long passageiroId)
        {
            if (_passageiros.Any(p => p.PassageiroId == passageiroId))
                throw new DomainException("Passageiro já está na viagem");
        }

        private void EnsurePassageiroExiste(ViagemPassageiro? viagemPassageiro)
        {
            if (viagemPassageiro == null)
                throw new DomainException("Passageiro não encontrado na viagem");
        }

        // Validações específicas de domínio - mantendo apenas as que são intrínsecas à entidade

        private void ValidarDataHoraPosicao(DateTime dataHora)
        {
            if (dataHora > DateTime.UtcNow)
                throw new DomainException("Data/hora não pode ser futura");
        }

        private void AtualizarVagasDisponiveis()
        {
            VagasDisponiveis = QuantidadeVagas - _passageiros.Count;
            Lotado = VagasDisponiveis <= 0;
        }

        protected override string DescricaoFormatada => $"{Codigo} - {DescricaoViagem}";
    }
}
