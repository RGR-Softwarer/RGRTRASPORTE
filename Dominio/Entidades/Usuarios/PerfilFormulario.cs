using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Usuarios
{
    internal class PerfilFormulario
    {

        
        public virtual long Codigo { get; set; }

        public virtual Dominio.Entidades.Usuarios.PerfilAcesso PerfilAcesso { get; set; }

        public virtual int CodigoFormulario { get; set; }

        public virtual bool SomenteLeitura { get; set; }

        public virtual ICollection<Dominio.Entidades.Usuarios.PerfilFormularioPermissaoPersonalizada> FormularioPermissaoPersonalizada { get; set; }
    }
}
