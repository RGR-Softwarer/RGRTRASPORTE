namespace Dominio.Entidades.Unidades
{
    internal class Setor
    {
        
        public virtual long Codigo { get; set; }

        public virtual string CodigoERP { get; set; }

        public virtual string Descricao { get; set; }

        public virtual bool Situacao { get; set; }

        #region Propriedades Virtuais

        public virtual string SituacaoFormatada
        {
            get
            {
                switch (this.Situacao)
                {
                    case true:
                        return "Ativo";
                    case false:
                        return "Inativo";
                    default:
                        return "";
                }
            }
        }

        #endregion

    }
}
