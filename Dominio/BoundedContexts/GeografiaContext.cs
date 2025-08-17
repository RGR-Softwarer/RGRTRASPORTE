using Dominio.BoundedContexts.SharedKernel;

namespace Dominio.BoundedContexts
{
    /// <summary>
    /// Bounded Context responsável pela gestão geográfica
    /// </summary>
    public class GeografiaContext : IBoundedContext
    {
        public string Name => "Geografia";
        
        public string Description => "Contexto responsável por gerenciar localidades, endereços e informações geográficas";
        
        public string Version => "1.0.0";

        /// <summary>
        /// Aggregate Roots pertencentes a este contexto
        /// </summary>
        public static readonly Type[] AggregateRoots = new[]
        {
            typeof(Dominio.Entidades.Localidades.Localidade)
        };

        /// <summary>
        /// Value Objects específicos deste contexto
        /// </summary>
        public static readonly Type[] ValueObjects = new[]
        {
            typeof(Dominio.ValueObjects.Endereco),
            typeof(Dominio.ValueObjects.Coordenada)
        };

        /// <summary>
        /// Domain Events publicados por este contexto
        /// </summary>
        public static readonly Type[] PublishedEvents = new[]
        {
            // Eventos serão adicionados conforme implementação  
            typeof(Dominio.Events.Base.DomainEvent) // Placeholder
        };

        /// <summary>
        /// Domain Events consumidos de outros contextos
        /// </summary>
        public static readonly Type[] ConsumedEvents = new[]
        {
            // Eventos serão adicionados conforme implementação
            typeof(Dominio.Events.Viagens.ViagemCriadaEvent) // Placeholder
        };

        /// <summary>
        /// Domain Services específicos deste contexto
        /// </summary>
        public static readonly Type[] DomainServices = new[]
        {
            typeof(Dominio.Services.LocalidadeValidationService)
        };
    }
}
