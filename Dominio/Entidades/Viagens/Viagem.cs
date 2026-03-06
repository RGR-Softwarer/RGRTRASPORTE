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
    /// Aggregate Root para entidade Viagem seguindo padr�es DDD.
    /// Representa uma viagem de transporte com passageiros, rotas e posicionamento.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa:
    /// - Factory Methods para cria��o controlada
    /// - Value Objects para tipos complexos
    /// - Domain Events para comunica��o
    /// - Specifications para regras de neg�cio
    /// - Notification Pattern para valida��es
    /// </remarks>
    public class Viagem : AggregateRoot
    {
        // Value Objects
        public CodigoViagem Codigo { get; private set; }
        public PeriodoViagem Periodo { get; private set; }
        public Distancia Distancia { get; private set; }
        public Polilinha PolilinhaRota { get; private set; }

        // Propriedades primitivas que n�o justificam Value Objects
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
        public TipoTrechoViagemEnum TipoTrecho { get; private set; }
        public long? ViagemParId { get; private set; }

        // Entidades filhas
        private readonly List<ViagemPassageiro> _passageiros = new();
        public IReadOnlyCollection<ViagemPassageiro> Passageiros => _passageiros.AsReadOnly();

        private readonly List<ViagemPosicao> _posicoes = new();
        public IReadOnlyCollection<ViagemPosicao> Posicoes => _posicoes.AsReadOnly();

        // Navega��o
        public Veiculo Veiculo { get; private set; }
        public Motorista Motorista { get; private set; }
        public Localidade LocalidadeOrigem { get; private set; }
        public Localidade LocalidadeDestino { get; private set; }
        public GatilhoViagem GatilhoViagem { get; private set; }
        public Viagem? ViagemPar { get; private set; }

        // Specifications para consultas b�sicas
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
            TipoTrechoViagemEnum tipoTrecho,
            bool ativo,
            long? gatilhoViagemId = null,
            long? viagemParId = null)
        {
            // Valida��es b�sicas de integridade - as regras de neg�cio complexas s�o validadas no ValidationService
            if (veiculoId <= 0 || motoristaId <= 0 || localidadeOrigemId <= 0 || localidadeDestinoId <= 0 || quantidadeVagas <= 0)
                throw new DomainException("Dados obrigat�rios n�o informados");

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
            TipoTrecho = tipoTrecho;
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;
            ViagemParId = viagemParId;
            Situacao = SituacaoViagemEnum.Agendada;
            Codigo = CodigoViagem.Gerar();

            AddDomainEvent(new ViagemCriadaEvent(Id, veiculoId, motoristaId, localidadeOrigemId, localidadeDestinoId));
        }

        // Factory Methods
        /// <summary>
        /// Cria uma nova viagem regular atrav�s de Factory Method.
        /// </summary>
        /// <param name="dataViagem">Data da viagem</param>
        /// <param name="horarioSaida">Hor�rio de sa�da</param>
        /// <param name="horarioChegada">Hor�rio de chegada</param>
        /// <param name="veiculoId">ID do ve�culo</param>
        /// <param name="motoristaId">ID do motorista</param>
        /// <param name="localidadeOrigemId">ID da localidade de origem</param>
        /// <param name="localidadeDestinoId">ID da localidade de destino</param>
        /// <param name="quantidadeVagas">Quantidade de vagas dispon�veis</param>
        /// <param name="distanciaKm">Dist�ncia em quil�metros</param>
        /// <param name="descricaoViagem">Descri��o da viagem</param>
        /// <param name="polilinhaRota">Polilinha da rota</param>
        /// <param name="ativo">Se a viagem est� ativa</param>
        /// <returns>Nova inst�ncia de Viagem</returns>
        /// <exception cref="DomainException">Lan�ada quando dados obrigat�rios n�o s�o informados</exception>
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
            TipoTrechoViagemEnum tipoTrecho,
            bool ativo = true,
            long? viagemParId = null)
        {
            var periodo = new PeriodoViagem(dataViagem, horarioSaida, horarioChegada);
            var distancia = new Distancia(distanciaKm);
            var polilinha = new Polilinha(polilinhaRota);

            return new Viagem(periodo, veiculoId, motoristaId, localidadeOrigemId, 
                localidadeDestinoId, quantidadeVagas, distancia, descricaoViagem, 
                polilinha, tipoTrecho, ativo, null, viagemParId);
        }

        // Factory Method com valida��o por NotificationContext
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
                    quantidadeVagas, distanciaKm, descricaoViagem, polilinhaRota, 
                    TipoTrechoViagemEnum.Ida, ativo);
                
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
            TipoTrechoViagemEnum tipoTrecho,
            bool ativo = true,
            long? viagemParId = null)
        {
            var periodo = new PeriodoViagem(dataViagem, gatilho.HorarioSaida, gatilho.HorarioChegada);
            var distancia = new Distancia(distanciaKm);
            var polilinha = new Polilinha(polilinhaRota);

            return new Viagem(periodo, veiculoId, motoristaId, 
                gatilho.LocalidadeOrigemId, gatilho.LocalidadeDestinoId, 
                quantidadeVagas, distancia, descricaoViagem, polilinha, 
                tipoTrecho, ativo, gatilho.Id, viagemParId);
        }

        // M�todos de neg�cio
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

        // M�todo de neg�cio com valida��o por NotificationContext
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

        // M�todo de neg�cio com valida��o por NotificationContext
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
            long? gatilhoViagemId = null,
            long? viagemParId = null)
        {
            EnsureViagemPodeSerEditada();

            var horariosAnteriores = (Periodo.HoraSaida, Periodo.HoraChegada);
            var novosPeriodo = new PeriodoViagem(dataViagem, horarioSaida, horarioChegada);

            // Valida��es b�sicas de integridade
            if (veiculoId <= 0 || localidadeOrigemId <= 0 || localidadeDestinoId <= 0 || quantidadeVagas <= 0)
                throw new DomainException("Dados obrigat�rios n�o informados");

            Periodo = novosPeriodo;
            VeiculoId = veiculoId;
            LocalidadeOrigemId = localidadeOrigemId;
            LocalidadeDestinoId = localidadeDestinoId;
            QuantidadeVagas = quantidadeVagas;
            VagasDisponiveis = quantidadeVagas - _passageiros.Count;
            Ativo = ativo;
            GatilhoViagemId = gatilhoViagemId;
            ViagemParId = viagemParId;
            
            UpdateTimestamp();

            AddDomainEvent(new ViagemAtualizadaEvent(Id));

            if (horariosAnteriores.HoraSaida != horarioSaida || horariosAnteriores.HoraChegada != horarioChegada)
            {
                AddDomainEvent(new HorarioViagemAlteradoEvent(Id, 
                    horariosAnteriores.HoraSaida, horariosAnteriores.HoraChegada,
                    horarioSaida, horarioChegada));
            }
        }

        // M�todo de atualiza��o com valida��o por NotificationContext
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

        // M�todos de consulta
        public bool TemPassageiro(long passageiroId) 
            => _passageiros.Any(p => p.PassageiroId == passageiroId);

        public int QuantidadePassageiros => _passageiros.Count;

        public ViagemPassageiro? ObterPassageiro(long passageiroId) 
            => _passageiros.FirstOrDefault(p => p.PassageiroId == passageiroId);

        public bool PodeReceberPassageiros() => Situacao == SituacaoViagemEnum.Agendada && VagasDisponiveis > 0;
        public bool PodeSerIniciada() => _podeSerIniciada.IsSatisfiedBy(this);
        public bool PodeSerFinalizada() => _podeSerFinalizada.IsSatisfiedBy(this);
        public bool PodeSerCancelada() => _podeSerCancelada.IsSatisfiedBy(this);

        // Valida��es usando Specifications
        private void EnsureViagemPodeReceberPassageiros()
        {
            if (Situacao != SituacaoViagemEnum.Agendada)
                throw new DomainException("Viagem deve estar agendada para receber passageiros");
            
            if (VagasDisponiveis <= 0)
                throw new DomainException("N�o h� vagas dispon�veis na viagem");
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
                throw new DomainException("Passageiro � obrigat�rio");
        }

        private void EnsurePassageiroNaoEstaNaviagem(long passageiroId)
        {
            if (_passageiros.Any(p => p.PassageiroId == passageiroId))
                throw new DomainException("Passageiro j� est� na viagem");
        }

        private void EnsurePassageiroExiste(ViagemPassageiro? viagemPassageiro)
        {
            if (viagemPassageiro == null)
                throw new DomainException("Passageiro n�o encontrado na viagem");
        }

        // Valida��es espec�ficas de dom�nio - mantendo apenas as que s�o intr�nsecas � entidade

        private void ValidarDataHoraPosicao(DateTime dataHora)
        {
            if (dataHora > DateTime.UtcNow)
                throw new DomainException("Data/hora n�o pode ser futura");
        }

        private void AtualizarVagasDisponiveis()
        {
            VagasDisponiveis = QuantidadeVagas - _passageiros.Count;
            Lotado = VagasDisponiveis <= 0;
        }

        protected override string DescricaoFormatada => $"{Codigo} - {DescricaoViagem}";
    }
}
