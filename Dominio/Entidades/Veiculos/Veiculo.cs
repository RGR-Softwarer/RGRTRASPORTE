using Dominio.Enums.Veiculo;
using Dominio.Events.Base;
using Dominio.Exceptions;
using Dominio.ValueObjects;
using Dominio.Specifications;
using Dominio.Services;
using Dominio.Interfaces;

namespace Dominio.Entidades.Veiculos
{
    public class Veiculo : AggregateRoot
    {
        // Specifications para validações básicas
        private static readonly VeiculoPodeSerAtivadoSpecification _podeSerAtivado = new();
        private static readonly VeiculoPodeSerInativadoSpecification _podeSerInativado = new();
        private static readonly VeiculoPodeSerEditadoSpecification _podeSerEditado = new();
        private static readonly VeiculoPodeAtualizarLicenciamentoSpecification _podeAtualizarLicenciamento = new();

        private Veiculo() { } // Para EF Core

        // Construtor privado - usar Factory Methods
        private Veiculo(
            Placa placa,
            string modelo,
            string marca,
            string numeroChassi,
            int anoModelo,
            int anoFabricacao,
            string cor,
            string renavam,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId)
        {
            // Validações básicas de integridade
            if (placa == null)
                throw new DomainException("Placa é obrigatória");

            if (string.IsNullOrWhiteSpace(modelo))
                throw new DomainException("Modelo é obrigatório");

            if (string.IsNullOrWhiteSpace(marca))
                throw new DomainException("Marca é obrigatória");

            Placa = placa;
            Modelo = modelo;
            Marca = marca;
            NumeroChassi = numeroChassi;
            AnoModelo = anoModelo;
            AnoFabricacao = anoFabricacao;
            Cor = cor;
            Renavam = renavam;
            VencimentoLicenciamento = vencimentoLicenciamento;
            TipoCombustivel = tipoCombustivel;
            Status = status;
            Observacao = observacao;
            ModeloVeiculoId = modeloVeiculoId;
            Situacao = true;

            AddDomainEvent(new VeiculoCriadoEvent(Id, placa.Numero, modelo, marca));
        }

        // Factory Methods
        public static Veiculo CriarVeiculo(
            Placa placa,
            string modelo,
            string marca,
            string numeroChassi,
            int anoModelo,
            int anoFabricacao,
            string cor,
            string renavam,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId)
        {
            return new Veiculo(placa, modelo, marca, numeroChassi, anoModelo, anoFabricacao,
                cor, renavam, vencimentoLicenciamento, tipoCombustivel, status, observacao, modeloVeiculoId);
        }

        // Factory Method com validação por NotificationContext
        public static (Veiculo? veiculo, bool sucesso) CriarVeiculoComValidacao(
            Placa placa,
            string modelo,
            string marca,
            string numeroChassi,
            int anoModelo,
            int anoFabricacao,
            string cor,
            string renavam,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId,
            IDomainNotificationContext notificationContext)
        {
            var validationService = new VeiculoValidationService();
            var valido = validationService.ValidarCriacao(placa, modelo, marca, numeroChassi,
                anoModelo, anoFabricacao, cor, renavam, vencimentoLicenciamento,
                tipoCombustivel, status, observacao, modeloVeiculoId, notificationContext);

            if (!valido)
                return (null, false);

            try
            {
                var veiculo = CriarVeiculo(placa, modelo, marca, numeroChassi, anoModelo,
                    anoFabricacao, cor, renavam, vencimentoLicenciamento, tipoCombustivel,
                    status, observacao, modeloVeiculoId);
                
                return (veiculo, true);
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return (null, false);
            }
        }

        public Placa Placa { get; private set; } = null!;
        public string Modelo { get; private set; } = null!;
        public string Marca { get; private set; } = null!;
        public string NumeroChassi { get; private set; } = null!;
        public int AnoModelo { get; private set; }
        public int AnoFabricacao { get; private set; }
        public string Cor { get; private set; } = null!;
        public string Renavam { get; private set; } = null!;
        public virtual DateTime? VencimentoLicenciamento { get; private set; }
        public TipoCombustivelEnum TipoCombustivel { get; private set; }
        public StatusVeiculoEnum Status { get; private set; }
        public string Observacao { get; private set; } = null!;
        public long? ModeloVeiculoId { get; private set; }
        public virtual ModeloVeicular ModeloVeiculo { get; private set; } = null!;
        public bool Situacao { get; private set; }

        public virtual string PlacaFormatada => Placa.Formatada;

        protected override string DescricaoFormatada => Placa.Formatada;

        public void Atualizar(
            string modelo,
            string marca,
            string cor,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId)
        {
            EnsureVeiculoPodeSerEditado();

            // Validações básicas de integridade
            if (string.IsNullOrWhiteSpace(modelo))
                throw new DomainException("Modelo é obrigatório");

            if (string.IsNullOrWhiteSpace(marca))
                throw new DomainException("Marca é obrigatória");

            if (string.IsNullOrWhiteSpace(cor))
                throw new DomainException("Cor é obrigatória");

            Modelo = modelo;
            Marca = marca;
            Cor = cor;
            VencimentoLicenciamento = vencimentoLicenciamento;
            TipoCombustivel = tipoCombustivel;
            Status = status;
            Observacao = observacao;
            ModeloVeiculoId = modeloVeiculoId;
            UpdateTimestamp();

            AddDomainEvent(new VeiculoAtualizadoEvent(Id, Placa.Numero));
        }

        // Método com validação por NotificationContext
        public bool AtualizarComValidacao(
            string modelo,
            string marca,
            string cor,
            DateTime? vencimentoLicenciamento,
            TipoCombustivelEnum tipoCombustivel,
            StatusVeiculoEnum status,
            string observacao,
            long? modeloVeiculoId,
            IDomainNotificationContext notificationContext)
        {
            var validationService = new VeiculoValidationService();
            var valido = validationService.ValidarAtualizacao(this, modelo, marca, cor,
                vencimentoLicenciamento, tipoCombustivel, status, observacao, modeloVeiculoId, notificationContext);

            if (!valido)
                return false;

            try
            {
                Atualizar(modelo, marca, cor, vencimentoLicenciamento, tipoCombustivel,
                    status, observacao, modeloVeiculoId);
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        public void AtualizarLicenciamento(DateTime vencimento)
        {
            EnsureVeiculoPodeAtualizarLicenciamento();
            EnsureVencimentoLicenciamentoValido(vencimento);

            VencimentoLicenciamento = vencimento;
            UpdateTimestamp();
            AddDomainEvent(new VeiculoLicenciamentoAtualizadoEvent(Id, Placa.Numero, vencimento));
        }

        // Método com validação por NotificationContext
        public bool AtualizarLicenciamentoComValidacao(DateTime vencimento, IDomainNotificationContext notificationContext)
        {
            var validationService = new VeiculoValidationService();
            var valido = validationService.ValidarAtualizacaoLicenciamento(this, vencimento, notificationContext);

            if (!valido)
                return false;

            try
            {
                AtualizarLicenciamento(vencimento);
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        public void Ativar()
        {
            EnsureVeiculoPodeSerAtivado();

            Situacao = true;
            UpdateTimestamp();
            AddDomainEvent(new VeiculoAtivadoEvent(Id, Placa.Numero));
        }

        // Método com validação por NotificationContext
        public bool AtivarComValidacao(IDomainNotificationContext notificationContext)
        {
            var validationService = new VeiculoValidationService();
            var valido = validationService.ValidarAtivacao(this, notificationContext);

            if (!valido)
                return false;

            try
            {
                Ativar();
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        public void Inativar()
        {
            EnsureVeiculoPodeSerInativado();

            Situacao = false;
            UpdateTimestamp();
            AddDomainEvent(new VeiculoInativadoEvent(Id, Placa.Numero));
        }

        // Método com validação por NotificationContext
        public bool InativarComValidacao(IDomainNotificationContext notificationContext)
        {
            var validationService = new VeiculoValidationService();
            var valido = validationService.ValidarInativacao(this, notificationContext);

            if (!valido)
                return false;

            try
            {
                Inativar();
                return true;
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return false;
            }
        }

        // Métodos de consulta
        public bool PodeSerAtivado() => _podeSerAtivado.IsSatisfiedBy(this);
        public bool PodeSerInativado() => _podeSerInativado.IsSatisfiedBy(this);
        public bool PodeSerEditado() => _podeSerEditado.IsSatisfiedBy(this);
        public bool PodeAtualizarLicenciamento() => _podeAtualizarLicenciamento.IsSatisfiedBy(this);

        // Validações usando Specifications
        private void EnsureVeiculoPodeSerAtivado()
        {
            if (!_podeSerAtivado.IsSatisfiedBy(this))
                throw new DomainException(_podeSerAtivado.ErrorMessage);
        }

        private void EnsureVeiculoPodeSerInativado()
        {
            if (!_podeSerInativado.IsSatisfiedBy(this))
                throw new DomainException(_podeSerInativado.ErrorMessage);
        }

        private void EnsureVeiculoPodeSerEditado()
        {
            if (!_podeSerEditado.IsSatisfiedBy(this))
                throw new DomainException(_podeSerEditado.ErrorMessage);
        }

        private void EnsureVeiculoPodeAtualizarLicenciamento()
        {
            if (!_podeAtualizarLicenciamento.IsSatisfiedBy(this))
                throw new DomainException(_podeAtualizarLicenciamento.ErrorMessage);
        }

        private void EnsureVencimentoLicenciamentoValido(DateTime vencimento)
        {
            var vencimentoValidoSpec = new VencimentoLicenciamentoValidoSpecification();
            if (!vencimentoValidoSpec.IsSatisfiedBy(vencimento))
                throw new DomainException(vencimentoValidoSpec.ErrorMessage);
        }
    }

    // Eventos de domínio para Veiculo
    public class VeiculoCriadoEvent : DomainEvent
    {
        public long VeiculoId { get; }
        public string Placa { get; }
        public string Modelo { get; }
        public string Marca { get; }

        public VeiculoCriadoEvent(long veiculoId, string placa, string modelo, string marca)
        {
            VeiculoId = veiculoId;
            Placa = placa;
            Modelo = modelo;
            Marca = marca;
        }
    }

    public class VeiculoAtualizadoEvent : DomainEvent
    {
        public long VeiculoId { get; }
        public string Placa { get; }

        public VeiculoAtualizadoEvent(long veiculoId, string placa)
        {
            VeiculoId = veiculoId;
            Placa = placa;
        }
    }

    public class VeiculoLicenciamentoAtualizadoEvent : DomainEvent
    {
        public long VeiculoId { get; }
        public string Placa { get; }
        public DateTime NovoVencimento { get; }

        public VeiculoLicenciamentoAtualizadoEvent(long veiculoId, string placa, DateTime novoVencimento)
        {
            VeiculoId = veiculoId;
            Placa = placa;
            NovoVencimento = novoVencimento;
        }
    }

    public class VeiculoAtivadoEvent : DomainEvent
    {
        public long VeiculoId { get; }
        public string Placa { get; }

        public VeiculoAtivadoEvent(long veiculoId, string placa)
        {
            VeiculoId = veiculoId;
            Placa = placa;
        }
    }

    public class VeiculoInativadoEvent : DomainEvent
    {
        public long VeiculoId { get; }
        public string Placa { get; }

        public VeiculoInativadoEvent(long veiculoId, string placa)
        {
            VeiculoId = veiculoId;
            Placa = placa;
        }
    }
}
