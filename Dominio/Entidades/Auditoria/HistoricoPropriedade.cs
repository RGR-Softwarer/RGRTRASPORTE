namespace Dominio.Entidades.Auditoria
{
    public class HistoricoPropriedade : BaseEntity
    {
        public HistoricoPropriedade() { }

        public HistoricoPropriedade(string propriedade, string de, string para)
        {
            this.Propriedade = propriedade;
            this.De = de;
            this.Para = para;
        }

        public HistoricoPropriedade(string propriedade, string de, string para, HistoricoObjeto historicoPai)
        {
            this.Propriedade = propriedade;
            this.De = de;
            this.Para = para;
            this.HistoricoObjeto = historicoPai;
        }

        public string Propriedade { get; set; }

        public string De { get; set; }

        public string Para { get; set; }

        public long HistoricoObjetoId { get; set; }

        public virtual HistoricoObjeto HistoricoObjeto { get; set; }
    }
}