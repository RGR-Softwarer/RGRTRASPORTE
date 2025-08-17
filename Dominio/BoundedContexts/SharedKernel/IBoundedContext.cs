namespace Dominio.BoundedContexts.SharedKernel
{
    /// <summary>
    /// Interface base para identificar Bounded Contexts
    /// </summary>
    public interface IBoundedContext
    {
        /// <summary>
        /// Nome único do contexto
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Descrição do propósito do contexto
        /// </summary>
        string Description { get; }
        
        /// <summary>
        /// Versão do contexto para compatibilidade
        /// </summary>
        string Version { get; }
    }
}
