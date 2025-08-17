using Dominio.BoundedContexts.SharedKernel;

namespace Dominio.BoundedContexts
{
    /// <summary>
    /// Bounded Context principal responsável por operações de transporte
    /// </summary>
    public class TransporteContext : IBoundedContext
    {
        public string Name => "Transporte";
        
        public string Description => "Contexto responsável por gerenciar viagens, rotas e operações de transporte";
        
        public string Version => "1.0.0";

        /// <summary>
        /// Aggregate Roots pertencentes a este contexto
        /// </summary>
        public static readonly Type[] AggregateRoots = new[]
        {
            typeof(Dominio.Entidades.Viagens.Viagem),
            typeof(Dominio.Entidades.Viagens.Gatilho.GatilhoViagem)
        };

        /// <summary>
        /// Value Objects específicos deste contexto
        /// </summary>
        public static readonly Type[] ValueObjects = new[]
        {
            typeof(Dominio.ValueObjects.CodigoViagem),
            typeof(Dominio.ValueObjects.PeriodoViagem),
            typeof(Dominio.ValueObjects.Polilinha),
            typeof(Dominio.ValueObjects.Distancia)
        };

        /// <summary>
        /// Domain Events publicados por este contexto
        /// </summary>
        public static readonly Type[] PublishedEvents = new[]
        {
            typeof(Dominio.Events.Viagens.ViagemCriadaEvent),
            typeof(Dominio.Events.Viagens.ViagemIniciadaEvent),
            typeof(Dominio.Events.Viagens.ViagemFinalizadaEvent),
            typeof(Dominio.Events.Viagens.ViagemCanceladaEvent)
        };

        /// <summary>
        /// Domain Events consumidos de outros contextos
        /// </summary>
        public static readonly Type[] ConsumedEvents = new[]
        {
            // Eventos serão adicionados conforme implementação
            typeof(Dominio.Events.Viagens.ViagemCriadaEvent) // Placeholder
        };
    }
}
