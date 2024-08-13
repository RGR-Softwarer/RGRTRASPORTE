using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Usuarios
{
    internal class Usuario
    {
        #region Propriedades

       
        public virtual long Codigo { get; set; }

        //public virtual Enumeradores.Pessoas.TipoPessoa TipoPessoa { get; set; }

        public virtual string CPF { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Login { get; set; }

        public virtual string Senha { get; set; }

        public virtual string Sessao { get; set; }

        public virtual DateTime? DataUltimoAcesso { get; set; }

        public virtual bool Administrador { get; set; }

        public virtual bool Ativo { get; set; }

        public virtual bool PermiteAuditar { get; set; }

        public virtual bool EnviarNotificacaoPorEmail { get; set; }

        public virtual bool PossuiRestricaoAcesso { get; set; }

        public virtual string Email { get; set; }

         public virtual ICollection<Dominio.Entidades.Unidades.Unidade> Unidades { get; set; }

        //public virtual Dominio.Entidades.Pessoas.Transportador Transportador { get; set; }

        public virtual Dominio.Entidades.Usuarios.PerfilAcesso PerfilAcesso { get; set; }

        #endregion

        #region Propriedades Virtuais

       /* public virtual string Descricao
        {
            get { return $"{Nome} ({CpfCnpjFormatado})"; }
        }*/

        public virtual string DescricaoAtivo
        {
            get { return Ativo ? "Ativo" : "Inativo"; }
        }

        /*public virtual string CpfCnpjFormatado
        {
            get { return TipoPessoa == Enumeradores.Pessoas.TipoPessoa.Juridica ? string.Format(@"{0:00\.000\.000\/0000\-00}", CPF.ToLong()) : string.Format(@"{0:000\.000\.000\-00}", CPF.ToLong()); }
        }*/

        #endregion
    }
}
