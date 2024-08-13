using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades.Usuarios
{
    internal class UsuarioFormulario
    {
       
        public virtual long Codigo { get; set; }

        public virtual Dominio.Entidades.Usuarios.Usuario Usuario { get; set; }

        public virtual int CodigoFormulario { get; set; }

        public virtual bool SomenteLeitura { get; set; }
  public virtual ICollection<Dominio.Entidades.Usuarios.UsuarioFormularioPermissaoPersonalizada> PermissoesPersonalizadas { get; set; }

    }
}
