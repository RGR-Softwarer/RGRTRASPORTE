using Dominio.Events.Base;

namespace Dominio.Entidades
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = "Sistema";
        public string UpdatedBy { get; set; } = "Sistema";

        protected virtual string DescricaoFormatada => Id.ToString();

        public virtual string DescricaoAuditoria => DescricaoFormatada;

        protected void UpdateTimestamp(string usuario = null)
        {
            UpdatedAt = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(usuario))
                UpdatedBy = usuario;
        }

        protected void SetCreatedBy(string usuario)
        {
            if (!string.IsNullOrEmpty(usuario))
            {
                CreatedBy = usuario;
                UpdatedBy = usuario;
            }
        }
    }

    public abstract class AggregateRoot : BaseEntity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
