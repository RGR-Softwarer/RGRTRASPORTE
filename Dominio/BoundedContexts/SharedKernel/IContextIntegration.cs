using Dominio.Events.Base;

namespace Dominio.BoundedContexts.SharedKernel
{
    /// <summary>
    /// Interface para comunicação entre Bounded Contexts
    /// </summary>
    public interface IContextIntegration
    {
        /// <summary>
        /// Publica um evento para outros contextos
        /// </summary>
        Task PublishEventAsync<T>(T domainEvent, CancellationToken cancellationToken = default)
            where T : DomainEvent;
        
        /// <summary>
        /// Subscreve para receber eventos de outros contextos
        /// </summary>
        void SubscribeToEvent<T>(Func<T, CancellationToken, Task> handler)
            where T : DomainEvent;
        
        /// <summary>
        /// Remove subscrição de eventos
        /// </summary>
        void UnsubscribeFromEvent<T>()
            where T : DomainEvent;
    }
}
