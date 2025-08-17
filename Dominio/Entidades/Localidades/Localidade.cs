using Dominio.Events.Base;
using Dominio.Exceptions;
using Dominio.ValueObjects;
using Dominio.Specifications;
using Dominio.Services;
using Dominio.Interfaces;

namespace Dominio.Entidades.Localidades
{
    public class Localidade : AggregateRoot
    {
        // Specifications para valida��es b�sicas
        private static readonly LocalidadePodeSerAtivadaSpecification _podeSerAtivada = new();
        private static readonly LocalidadePodeSerInativadaSpecification _podeSerInativada = new();
        private static readonly LocalidadePodeSerEditadaSpecification _podeSerEditada = new();

        private Localidade() { } // Para EF Core

        // Construtor privado - usar Factory Methods
        private Localidade(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude)
        {
            // Valida��es b�sicas de integridade
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome � obrigat�rio");

            if (endereco == null)
                throw new DomainException("Endere�o � obrigat�rio");

            Nome = nome;
            Endereco = endereco;
            Latitude = latitude;
            Longitude = longitude;
            Ativo = true;

            AddDomainEvent(new LocalidadeCriadaEvent(Id, nome));
        }

        // Factory Methods
        public static Localidade CriarLocalidade(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude)
        {
            return new Localidade(nome, endereco, latitude, longitude);
        }

        // Factory Method com valida��o por NotificationContext
        public static (Localidade? localidade, bool sucesso) CriarLocalidadeComValidacao(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude,
            IDomainNotificationContext notificationContext)
        {
            var validationService = new LocalidadeValidationService();
            var valido = validationService.ValidarCriacao(nome, endereco, latitude, longitude, notificationContext);

            if (!valido)
                return ((Localidade?)null, false);

            try
            {
                var localidade = CriarLocalidade(nome, endereco, latitude, longitude);
                return (localidade, true);
            }
            catch (DomainException ex)
            {
                notificationContext.AddNotification(ex.Message);
                return ((Localidade?)null, false);
            }
        }

        public string Nome { get; private set; } = null!;
        public Endereco Endereco { get; private set; } = null!;
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }
        public bool Ativo { get; private set; }

        // Propriedades de conveni�ncia para compatibilidade com EF
        public string Estado => Endereco.Estado;
        public string Cidade => Endereco.Cidade;
        public string Cep => Endereco.Cep;
        public string Bairro => Endereco.Bairro;
        public string Logradouro => Endereco.Logradouro;
        public string Numero => Endereco.Numero;
        public string Complemento => Endereco.Complemento;
        public string Uf => Endereco.Uf;
        public string EnderecoCompleto => Endereco.EnderecoCompleto;

        public void Atualizar(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude,
            bool ativo)
        {
            EnsureLocalidadePodeSerEditada();

            // Valida��es b�sicas de integridade
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome � obrigat�rio");

            if (endereco == null)
                throw new DomainException("Endere�o � obrigat�rio");

            Nome = nome;
            Endereco = endereco;
            Latitude = latitude;
            Longitude = longitude;
            Ativo = ativo;
            UpdateTimestamp();

            AddDomainEvent(new LocalidadeAtualizadaEvent(Id, nome));
        }

        // M�todo com valida��o por NotificationContext
        public bool AtualizarComValidacao(
            string nome,
            Endereco endereco,
            decimal latitude,
            decimal longitude,
            bool ativo,
            IDomainNotificationContext notificationContext)
        {
            var validationService = new LocalidadeValidationService();
            var valido = validationService.ValidarAtualizacao(this, nome, endereco, latitude, longitude, notificationContext);

            if (!valido)
                return false;

            try
            {
                Atualizar(nome, endereco, latitude, longitude, ativo);
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
            EnsureLocalidadePodeSerAtivada();

            Ativo = true;
            UpdateTimestamp();
            AddDomainEvent(new LocalidadeAtivadaEvent(Id, Nome));
        }

        // M�todo com valida��o por NotificationContext
        public bool AtivarComValidacao(IDomainNotificationContext notificationContext)
        {
            var validationService = new LocalidadeValidationService();
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
            EnsureLocalidadePodeSerInativada();

            Ativo = false;
            UpdateTimestamp();
            AddDomainEvent(new LocalidadeInativadaEvent(Id, Nome));
        }

        // M�todo com valida��o por NotificationContext
        public bool InativarComValidacao(IDomainNotificationContext notificationContext)
        {
            var validationService = new LocalidadeValidationService();
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

        // M�todos de consulta
        public bool PodeSerAtivada() => _podeSerAtivada.IsSatisfiedBy(this);
        public bool PodeSerInativada() => _podeSerInativada.IsSatisfiedBy(this);
        public bool PodeSerEditada() => _podeSerEditada.IsSatisfiedBy(this);

        // Valida��es usando Specifications
        private void EnsureLocalidadePodeSerAtivada()
        {
            if (!_podeSerAtivada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerAtivada.ErrorMessage);
        }

        private void EnsureLocalidadePodeSerInativada()
        {
            if (!_podeSerInativada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerInativada.ErrorMessage);
        }

        private void EnsureLocalidadePodeSerEditada()
        {
            if (!_podeSerEditada.IsSatisfiedBy(this))
                throw new DomainException(_podeSerEditada.ErrorMessage);
        }

        protected override string DescricaoFormatada => $"{Nome} - {Endereco.Cidade}/{Endereco.Estado}";
    }

    // Eventos de dom�nio para Localidade
    public class LocalidadeCriadaEvent : DomainEvent
    {
        public long LocalidadeId { get; }
        public string Nome { get; }

        public LocalidadeCriadaEvent(long localidadeId, string nome)
        {
            LocalidadeId = localidadeId;
            Nome = nome;
        }
    }

    public class LocalidadeAtualizadaEvent : DomainEvent
    {
        public long LocalidadeId { get; }
        public string Nome { get; }

        public LocalidadeAtualizadaEvent(long localidadeId, string nome)
        {
            LocalidadeId = localidadeId;
            Nome = nome;
        }
    }

    public class LocalidadeAtivadaEvent : DomainEvent
    {
        public long LocalidadeId { get; }
        public string Nome { get; }

        public LocalidadeAtivadaEvent(long localidadeId, string nome)
        {
            LocalidadeId = localidadeId;
            Nome = nome;
        }
    }

    public class LocalidadeInativadaEvent : DomainEvent
    {
        public long LocalidadeId { get; }
        public string Nome { get; }

        public LocalidadeInativadaEvent(long localidadeId, string nome)
        {
            LocalidadeId = localidadeId;
            Nome = nome;
        }
    }
}
