using Dominio.BoundedContexts.SharedKernel;

namespace Dominio.BoundedContexts
{
    /// <summary>
    /// Registro central de todos os Bounded Contexts da aplicação
    /// </summary>
    public static class ContextRegistry
    {
        /// <summary>
        /// Todos os contextos registrados na aplicação
        /// </summary>
        public static readonly IBoundedContext[] AllContexts = new IBoundedContext[]
        {
            new TransporteContext(),
            new FrotaContext(),
            new PessoasContext(),
            new GeografiaContext()
        };

        /// <summary>
        /// Obtém um contexto pelo nome
        /// </summary>
        public static IBoundedContext? GetContextByName(string name)
        {
            return AllContexts.FirstOrDefault(c => 
                string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Obtém o contexto que contém um determinado Aggregate Root
        /// </summary>
        public static IBoundedContext? GetContextForAggregateRoot<T>()
            where T : Dominio.Entidades.BaseEntity
        {
            var aggregateType = typeof(T);
            
            if (TransporteContext.AggregateRoots.Contains(aggregateType))
                return new TransporteContext();
                
            if (FrotaContext.AggregateRoots.Contains(aggregateType))
                return new FrotaContext();
                
            if (PessoasContext.AggregateRoots.Contains(aggregateType))
                return new PessoasContext();
                
            if (GeografiaContext.AggregateRoots.Contains(aggregateType))
                return new GeografiaContext();
                
            return null;
        }

        /// <summary>
        /// Obtém todos os contextos que publicam um determinado evento
        /// </summary>
        public static IEnumerable<IBoundedContext> GetContextsPublishingEvent<T>()
            where T : Dominio.Events.Base.DomainEvent
        {
            var eventType = typeof(T);
            var publishingContexts = new List<IBoundedContext>();
            
            if (TransporteContext.PublishedEvents.Contains(eventType))
                publishingContexts.Add(new TransporteContext());
                
            if (FrotaContext.PublishedEvents.Contains(eventType))
                publishingContexts.Add(new FrotaContext());
                
            if (PessoasContext.PublishedEvents.Contains(eventType))
                publishingContexts.Add(new PessoasContext());
                
            if (GeografiaContext.PublishedEvents.Contains(eventType))
                publishingContexts.Add(new GeografiaContext());
                
            return publishingContexts;
        }

        /// <summary>
        /// Obtém todos os contextos que consomem um determinado evento
        /// </summary>
        public static IEnumerable<IBoundedContext> GetContextsConsumingEvent<T>()
            where T : Dominio.Events.Base.DomainEvent
        {
            var eventType = typeof(T);
            var consumingContexts = new List<IBoundedContext>();
            
            if (TransporteContext.ConsumedEvents.Contains(eventType))
                consumingContexts.Add(new TransporteContext());
                
            if (FrotaContext.ConsumedEvents.Contains(eventType))
                consumingContexts.Add(new FrotaContext());
                
            if (PessoasContext.ConsumedEvents.Contains(eventType))
                consumingContexts.Add(new PessoasContext());
                
            if (GeografiaContext.ConsumedEvents.Contains(eventType))
                consumingContexts.Add(new GeografiaContext());
                
            return consumingContexts;
        }

        /// <summary>
        /// Valida se todos os contextos estão corretamente configurados
        /// </summary>
        public static (bool IsValid, List<string> Errors) ValidateContexts()
        {
            var errors = new List<string>();
            var isValid = true;

            foreach (var context in AllContexts)
            {
                // Validar nome único
                var duplicateNames = AllContexts
                    .Where(c => c != context && c.Name == context.Name)
                    .ToList();
                    
                if (duplicateNames.Any())
                {
                    errors.Add($"Contexto '{context.Name}' possui nomes duplicados");
                    isValid = false;
                }

                // Validar propriedades obrigatórias
                if (string.IsNullOrWhiteSpace(context.Name))
                {
                    errors.Add("Contexto com nome vazio encontrado");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(context.Description))
                {
                    errors.Add($"Contexto '{context.Name}' sem descrição");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(context.Version))
                {
                    errors.Add($"Contexto '{context.Name}' sem versão");
                    isValid = false;
                }
            }

            return (isValid, errors);
        }

        /// <summary>
        /// Gera relatório dos contextos e suas responsabilidades
        /// </summary>
        public static string GenerateContextReport()
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("=== BOUNDED CONTEXTS REPORT ===");
            report.AppendLine();

            foreach (var context in AllContexts)
            {
                report.AppendLine($"Context: {context.Name} (v{context.Version})");
                report.AppendLine($"Description: {context.Description}");
                
                // Adicionar informações específicas se disponíveis
                switch (context)
                {
                    case TransporteContext:
                        report.AppendLine($"Aggregates: {TransporteContext.AggregateRoots.Length}");
                        report.AppendLine($"Value Objects: {TransporteContext.ValueObjects.Length}");
                        report.AppendLine($"Published Events: {TransporteContext.PublishedEvents.Length}");
                        report.AppendLine($"Consumed Events: {TransporteContext.ConsumedEvents.Length}");
                        break;
                    case FrotaContext:
                        report.AppendLine($"Aggregates: {FrotaContext.AggregateRoots.Length}");
                        report.AppendLine($"Value Objects: {FrotaContext.ValueObjects.Length}");
                        report.AppendLine($"Published Events: {FrotaContext.PublishedEvents.Length}");
                        report.AppendLine($"Consumed Events: {FrotaContext.ConsumedEvents.Length}");
                        break;
                    case PessoasContext:
                        report.AppendLine($"Aggregates: {PessoasContext.AggregateRoots.Length}");
                        report.AppendLine($"Value Objects: {PessoasContext.ValueObjects.Length}");
                        report.AppendLine($"Published Events: {PessoasContext.PublishedEvents.Length}");
                        report.AppendLine($"Consumed Events: {PessoasContext.ConsumedEvents.Length}");
                        break;
                    case GeografiaContext:
                        report.AppendLine($"Aggregates: {GeografiaContext.AggregateRoots.Length}");
                        report.AppendLine($"Value Objects: {GeografiaContext.ValueObjects.Length}");
                        report.AppendLine($"Published Events: {GeografiaContext.PublishedEvents.Length}");
                        report.AppendLine($"Consumed Events: {GeografiaContext.ConsumedEvents.Length}");
                        break;
                }
                
                report.AppendLine();
            }

            return report.ToString();
        }
    }
}
