using Dominio.BoundedContexts.SharedKernel;

namespace Dominio.BoundedContexts
{
    /// <summary>
    /// Bounded Context responsável pela gestão da frota de veículos
    /// </summary>
    public class FrotaContext : IBoundedContext
    {
        public string Name => "Frota";
        
        public string Description => "Contexto responsável por gerenciar veículos, modelos e capacidades da frota";
        
        public string Version => "1.0.0";

        /// <summary>
        /// Aggregate Roots pertencentes a este contexto
        /// </summary>
        public static readonly Type[] AggregateRoots = new[]
        {
            typeof(Dominio.Entidades.Veiculos.Veiculo),
            typeof(Dominio.Entidades.Veiculos.ModeloVeicular)
        };

        /// <summary>
        /// Value Objects específicos deste contexto
        /// </summary>
        public static readonly Type[] ValueObjects = new[]
        {
            typeof(Dominio.ValueObjects.Placa),
            typeof(Dominio.ValueObjects.Chassi),
            typeof(Dominio.ValueObjects.CapacidadeVeiculo)
        };

        /// <summary>
        /// Domain Events publicados por este contexto
        /// </summary>
        public static readonly Type[] PublishedEvents = new[]
        {
            typeof(Dominio.Events.Veiculos.ModeloVeicularCriadoEvent) // Eventos existentes
        };

        /// <summary>
        /// Domain Events consumidos de outros contextos
        /// </summary>
        public static readonly Type[] ConsumedEvents = new[]
        {
            // Eventos do contexto de Transporte para validações
            typeof(Dominio.Events.Viagens.ViagemCriadaEvent)
        };

        /// <summary>
        /// Domain Services específicos deste contexto
        /// </summary>
        public static readonly Type[] DomainServices = new[]
        {
            typeof(Dominio.Services.VeiculoValidationService),
            typeof(Dominio.Services.ModeloVeicularValidationService)
        };
    }
}
