using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Usuarios
{
    internal class PerfilAcesso
    {
         public virtual long Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual bool Situacao { get; set; }

        public virtual bool PerfilAdministrador { get; set; }

         public virtual ICollection<int> ModulosLiberados { get; set; }

         public virtual ICollection<Dominio.Entidades.Usuarios.PerfilFormulario> FormulariosLiberados { get; set; }

        public virtual string CodigoIntegracao { get; set; }

        public virtual string DescricaoSituacao
        {
            get { return Situacao ? "Ativo" : "Inativo"; }
        }
    }
}
