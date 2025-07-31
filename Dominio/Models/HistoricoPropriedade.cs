namespace Dominio.Models
{
    /// <summary>
    /// Record para representar alterações de propriedades na auditoria.
    /// Usado apenas como modelo interno para cálculo de diferenças.
    /// </summary>
    public record HistoricoPropriedade(string Propriedade, string De, string Para);
} 
