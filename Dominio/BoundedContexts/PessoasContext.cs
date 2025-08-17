using Dominio.BoundedContexts.SharedKernel;

namespace Dominio.BoundedContexts
{
    /// <summary>
    /// Bounded Context responsável pela gestão de pessoas
    /// </summary>
    public class PessoasContext : IBoundedContext
    {
        public string Name => "Pessoas";
        
        public string Description => "Contexto responsável por gerenciar motoristas, passageiros e suas informações pessoais";
        
        public string Version => "1.0.0";

        /// <summary>
        /// Aggregate Roots pertencentes a este contexto
        /// </summary>
        public static readonly Type[] AggregateRoots = new[]
        {
            typeof(Dominio.Entidades.Pessoas.Motorista),
            typeof(Dominio.Entidades.Pessoas.Passageiros.Passageiro)
        };

        /// <summary>
        /// Value Objects específicos deste contexto
        /// </summary>
        public static readonly Type[] ValueObjects = new[]
        {
            typeof(Dominio.ValueObjects.CPF),
            typeof(Dominio.ValueObjects.CNH)
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
            typeof(Dominio.Services.PessoaValidationService),
            typeof(Dominio.Services.MotoristaValidationService),
            typeof(Dominio.Services.PassageiroValidationService)
        };
    }
}
